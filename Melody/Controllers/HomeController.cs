using Melody.Data;
using Melody.Filters;
using Melody.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Melody.Controllers
{
    public class HomeController : Controller
    {
        private readonly MelodyContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(MelodyContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        //Index
        [AllowAnonymous]
        public IActionResult Index()
        {
            var artists = _context.Artists.ToList();
            var albums = _context.Albums.ToList();
            var podcasts = _context.Podcasts.ToList();
            var playlists = _context.Playlists.ToList();

            var viewModel = new HomeViewModel
            {
                Artists = artists,
                Albums = albums,
                Podcasts = podcasts,
                Playlists = playlists
            };

            return View(viewModel);
        }

        /*[SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult Profile()
        {
            //fetching user by email
            var email = HttpContext.Session.GetString("UserEmail");

            // Retrieving the user's details using the email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var artists = _context.Artists.ToList();
            var albums = _context.Albums.ToList();
            var podcasts = _context.Podcasts.ToList();
            var playlists = _context.Playlists.ToList();

            var viewModel = new HomeViewModel
            {
                UserDetails = user,
                Artists = artists,
                Albums = albums,
                Podcasts = podcasts,
                Playlists = playlists
            };

            return View(viewModel);
        }*/
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult Profile()
        {
            // Fetching user by email from the session
            var email = HttpContext.Session.GetString("UserEmail");

            // Retrieving the user's details using the email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Fetch only the playlists belonging to the logged-in user
            var userPlaylists = _context.Playlists
                                        .Where(p => p.UserId == user.UserId)
                                        .ToList();
            var Following = _context.Following
                             .Include(f => f.Artist) // Include Artist data
                             .Where(f => f.UserId == user.UserId)
                             .ToList();

            var viewModel = new HomeViewModel
            {
                UserDetails = user,
                Playlists = userPlaylists,
                Followings=Following
            };

            return View(viewModel);
        }



        // Settings
        [SubscriptionAuthorize("Free","Bronze","Silver","Gold")]
        public IActionResult Settings(string lang = "en")
        {
            ViewData["SelectedLanguage"] = lang;
            return View();
        }


        //Artists
        //[SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        [AllowAnonymous]
        public IActionResult Artists()
        {
            var artists = _context.Artists.ToList();
            return View(artists);
        }



        /// <summary>
        /// Search by Artist, Album, Song name
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //[SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Message = "Please enter a search term.";
                return View(new SearchResultViewModel());
            }

            var searchResults = new SearchResultViewModel();
            var loweredQuery = query.ToLower();

            // Search for album
            var album = await _context.Albums
                                       .FirstOrDefaultAsync(a => a.Title.ToLower().Contains(loweredQuery));
            if (album != null)
            {
                searchResults.Albums.Add(album);
                ViewBag.ResultType = "Album";  // Set the result type for the view
                return View(searchResults);    // Return the view with the album data
            }

            // Search for artist
            var artist = await _context.Artists
                                        .FirstOrDefaultAsync(a => a.Name.ToLower().Contains(loweredQuery));
            if (artist != null)
            {
                searchResults.Artists.Add(artist);
                ViewBag.ResultType = "Artist";  // Set the result type for the view
                return View(searchResults);     // Return the view with the artist data
            }

            // Search for song
            var song = await _context.Songs
                                      .Include(s => s.Album) // Include Album details to show in the view
                                      .FirstOrDefaultAsync(s => s.Name.ToLower().Contains(loweredQuery));
            if (song != null)
            {
                searchResults.Songs.Add(song);
                ViewBag.ResultType = "Song";  // Set the result type for the view
                return View(searchResults);   // Return the view with the song data
            }

            // No results found
            ViewBag.Message = "No results found for your search.";
            ViewBag.ResultType = "None";  // No results found
            return View(searchResults);
        }


        //Songslist
        //[SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        [AllowAnonymous]
        public IActionResult Songslist()
        {
            var songs = _context.Songs.Include(s => s.Album).ToList();
            var searchResults = new SearchResultViewModel
            {
                Songs = songs
            };

            return View(searchResults);   // Return the view with the song data
        }


        //Notification
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult Notification()
        {
            return View();
        }


        //ProfileUpdate-GET
        [HttpGet]
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult ProfileUpdate()
        {
            // Get the user email from the session
            var email = HttpContext.Session.GetString("UserEmail");

            // Find the user by email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return View(user);
        }


        //ProfileUpdate-POST
        [HttpPost]
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public async Task<IActionResult> ProfileUpdate(string Firstname, string Lastname, IFormFile ProfileImage)
        {
            // Get the user email from the session
            var email = HttpContext.Session.GetString("UserEmail");

            // Find the user by email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update the firstname and lastname
            user.Firstname = Firstname;
            user.Lastname = Lastname;

            // Check if a profile image was uploaded
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Create a unique filename for the image
                var fileName = Path.GetFileNameWithoutExtension(ProfileImage.FileName);
                var extension = Path.GetExtension(ProfileImage.FileName);
                var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                // Define the path to save the image in the wwwroot/images folder
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                // Ensure the directory exists
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                }

                // Save the file
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Update the user's profile image path (relative to wwwroot)
                user.ProfileImagePath = $"/images/{newFileName}";
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("ProfileUpdate");
        }



        //ChangePassword-GET
        [HttpGet]
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult ChangePassword()
        {
            return View();
        }


        //ChangePassword-POST
        [HttpPost]
        [SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public IActionResult ChangePassword(string OldPassword,string NewPassword, string ConfirmPassword)
        {
            // Get the user email from the session
            var email = HttpContext.Session.GetString("UserEmail");

            // Find the user by email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the old password matches
            if (user.Password != OldPassword)
            {
                ViewBag.OldPasswordError = "Old password is incorrect.";
                return View();
            }

            // Check if the passwords match
            if (NewPassword != ConfirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            // Update the password
            user.Password = NewPassword;

            _context.SaveChanges();

            return RedirectToAction("Profile");
        }


        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Signup");
        }
    }
}
