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
        [AllowAnonymous]
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

        [SubscriptionAuthorize("Free")]
        public IActionResult Profile()
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

        // Settings
        [SubscriptionAuthorize("Free")]
        public IActionResult Settings(string lang = "en")
        {
            ViewData["SelectedLanguage"] = lang;
            return View();
        }

        // Ensure only subscribed users can view the artists page
        [SubscriptionAuthorize("Free")]
        public IActionResult Artists()
        {
            var artists = _context.Artists.ToList();
            return View(artists);
        }

        [SubscriptionAuthorize("Free")]
        public IActionResult Playlist()
        {
            return RedirectToAction("Index", "Playlists");
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
        public IActionResult Songslist()
        {
            // Fetch all songs from the database
            var songs = _context.Songs.Include(s => s.Album).Include(s => s.Artist).ToList(); // Include related data if necessary
            var searchResults = new SearchResultViewModel
            {
                Songs = songs
            };

            return View(searchResults); // Return the view with the song data
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Signup");
        }
    }
}
