using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            var artists = _context.Artists.ToList(); // Retrieve artists from the database
            var albums = _context.Albums.ToList();   // Retrieve albums from the database

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

        public IActionResult Account()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
