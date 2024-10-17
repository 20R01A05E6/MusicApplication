using Melody.Data;
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

        //[NonAction]
        //[Authorize]
        public IActionResult Index()
        {
            var artists = _context.Artists.ToList();
            var albums = _context.Albums.ToList();

            var viewModel = new HomeViewModel
            {
                Artists = artists,
                Albums = albums
            };

            return View(viewModel);
        }

        public IActionResult Artists()
        {
            return RedirectToAction("Index","Artists");
        }

        public IActionResult Albums()
        {
            // Redirect to Index action in AlbumsController
            return RedirectToAction("Index", "Albums");
        }

        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> Search(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return View(new List<SearchResultViewModel>());
            }

            // Searching in the Albums, Artists, and Songs tables
            var albums = await _context.Albums
                                       .Where(a => a.Title.Contains(text))
                                       .ToListAsync();
            var artists = await _context.Artists
                                        .Where(a => a.Name.Contains(text))
                                        .ToListAsync();
            var songs = await _context.Songs
                                      .Where(s => s.Name.Contains(text))
                                      .ToListAsync();

            // Combine the results into a ViewModel
            var searchResults = new SearchResultViewModel
            {
                Albums = albums,
                Artists = artists,
                Songs = songs
            };

            return View(searchResults);
        }

    public IActionResult Logout()
        {
            return RedirectToAction("Login", "Signup");
        }
    }
}
