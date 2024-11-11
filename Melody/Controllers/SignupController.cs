using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;

namespace Melody.Controllers
{
    public class SignupController : Controller
    {
        private readonly MelodyContext _context;
        private readonly IConfiguration _configuration;

        public SignupController(MelodyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Signup
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Signup
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(Signup model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.UserDetails.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(model);
                }
                // Hash the password before saving it to the database
                var passwordHasher = new PasswordHasher<UserDetails>();
                var hashedPassword = passwordHasher.HashPassword(null, model.Password);

                var newUser = new UserDetails
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Password = hashedPassword, // Store plain text for now (hash in production)
                    SubscriptionType = "Free" // Default subscription type
                };

                _context.UserDetails.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Signup");
            }

            return View(model);
        }

        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("", "Email and Password cannot be empty.");
                return View();
            }

            var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist.");
                return View();
            }
            // Verify the hashed password
            var passwordHasher = new PasswordHasher<UserDetails>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, Password);

            if (result == PasswordVerificationResult.Success)
            {
                // Set session
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType);
                HttpContext.Session.SetString("ProfileImagePath", user.ProfileImagePath ?? "/images/user_logo.jpg");

                // Optionally generate a JWT token
                var token = GenerateJwtToken(user);
                // Store the JWT token in a cookie (optional, we can also store in session)
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                return RedirectToAction("Index", "Home");
                // Return the token in the response body
                //return Ok(new
                //{
                //    Token = token,
                //    Message = "Login successful"
                //});
            }
            else
            {
                // Password is incorrect
                ModelState.AddModelError("Password", "Password did not match.");
                return View();
            }
        }

        private string GenerateJwtToken(UserDetails user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.SubscriptionType) // Using SubscriptionType as role
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task GoogleSignin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse", "Signup")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal?.Identities.FirstOrDefault()?.Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            return RedirectToAction("Index", "Home");
        }


        // Access Denied page
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }



        // Subscription
        [AllowAnonymous]
        public IActionResult Subscription()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscription(string newSubscriptionType)
        {
            // Retrieve UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var user = await _context.UserDetails.FindAsync(userId);

                if (user != null)
                {
                    // Update the subscription type
                    user.SubscriptionType = newSubscriptionType;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Subscription");
            }

            return Unauthorized();
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Forgot(UserDetails model)
        {
            var user = await _context.UserDetails.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null)
            {
                // Generate a new password
                string newPassword = RandomPassword();

                // Hash the new password
                var passwordHasher = new PasswordHasher<UserDetails>();
                user.Password = passwordHasher.HashPassword(user, newPassword);

                _context.Update(user);
                await _context.SaveChangesAsync();

                // Send the new password to the user's email
                SendResetPasswordEmail(user.Email, newPassword);

                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("Email", "Email does not exist.");
            }
            return View();
        }


        private void SendResetPasswordEmail(string email, string newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("purandharkola@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Password Reset Request";
                mail.Body = $"Your new password is: {newPassword}";

                //MailBee.SmtpMail.Smtp.QuickSend("jdoe@domain.com", email , sub, "Message Body");

                SmtpClient smtpServer = new SmtpClient();

                smtpServer.Host = "smtp.gmail.com";
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("purandharkola@gmail.com", "ozkx uaox igna ttkz");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                // Handle exception (log it or display an error message)
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }

        //Random Password generator
        private string RandomPassword()
        {
            StringBuilder newpass = new StringBuilder();
            Random ranpass = new Random();
            string validChar = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_-+=`~<>?/'':;[]{}";
            for (int i = 0; i < 10; i++)
            {
                newpass.Append(validChar[ranpass.Next(validChar.Length)]);
            }
            return newpass.ToString();
        }

    }
}
