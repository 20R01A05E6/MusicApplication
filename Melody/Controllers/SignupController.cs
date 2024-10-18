using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;

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
        public IActionResult Signup()
        {
            return View(); // Return the signup view
        }

        // POST: Signup
        [HttpPost]
        public async Task<IActionResult> Signup(Signup model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.UserDetails.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(model); // Return the view with error messages
                }

                var newUser = new UserDetails
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Email = model.Email,
                    Password = model.Password // Store plain text for now (hash in production)
                };

                _context.UserDetails.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Signup"); // Redirect to login on successful signup
            }

            return View(model); // Return the view with validation errors
        }



        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
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

            if (user.Password != Password) // Should hash the password for security in production
            {
                ModelState.AddModelError("Password", "Password did not match.");
                return View();
            }
            HttpContext.Session.SetInt32("UserId", user.UserId);
            return RedirectToAction("Subscription", "Signup");
        }

        public IActionResult Subscription()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
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
