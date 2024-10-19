using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Melody.Filters;

namespace Melody.Controllers
{
    public class SignupController : Controller
    {
        private readonly MelodyContext _context;

        public SignupController(MelodyContext context)
        {
            _context = context;
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

                var newUser = new UserDetails
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Password = model.Password, // Store plain text for now (hash in production)
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

            if (user.Password != Password)
            {
                ModelState.AddModelError("Password", "Password did not match.");
                return View();
            }

            // Set session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("SubscriptionType", user.SubscriptionType);

            return RedirectToAction("Index", "Home");
        }


        // Access Denied page
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Signup");
        }

        // Subscription
        [Authorize]
        public IActionResult Subscription()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscription(string newSubscriptionType)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value); // Get UserId from claims

            var user = await _context.UserDetails.FindAsync(userId);

            if (user != null)
            {
                user.SubscriptionType = newSubscriptionType;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Subscription");
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
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist.");
                return View();
            }
            return View();
        }

    }
}
