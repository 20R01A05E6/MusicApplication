using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        public IActionResult Index()
        {
            var artists = _context.Artists.ToList();
            var albums = _context.Albums.ToList();
            var podcasts = _context.Podcasts.ToList();

            var viewModel = new HomeViewModel
            {
                Artists = artists,
                Albums = albums,
                Podcasts = podcasts
            };

            return View(viewModel);
        }

       public IActionResult Notification()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        //Settings
        public IActionResult Settings(string lang = "en")
        {
            ViewData["SelectedLanguage"] = lang;

            return View();
        }


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
                Console.WriteLine("Album found: " + album.Title);
                ViewBag.ResultType = "Album";  // Set a flag for the view
                return View(searchResults);    // Return the view with the album
            }

            // Search for artist
            var artist = await _context.Artists
                                        .FirstOrDefaultAsync(a => a.Name.ToLower().Contains(loweredQuery));
            if (artist != null)
            {
                searchResults.Artists.Add(artist);
                Console.WriteLine("Artist found: " + artist.Name);
                ViewBag.ResultType = "Artist"; 
                return View(searchResults);  
            }

            // Search for song
            var song = await _context.Songs
                                      .FirstOrDefaultAsync(s => s.Name.ToLower().Contains(loweredQuery));
            if (song != null)
            {
                searchResults.Songs.Add(song);
                Console.WriteLine("Song found: " + song.Name);
                ViewBag.ResultType = "Song";
                return View(searchResults);
            }

            // No results found
            ViewBag.Message = "No results found for your search.";
            ViewBag.ResultType = "None";  // No results found flag
            return View(new SearchResultViewModel());
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Signup");
        }
    }
}
