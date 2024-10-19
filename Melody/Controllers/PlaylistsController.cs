using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;

namespace Melody.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly MelodyContext _context;

        public PlaylistsController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Playlists
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Playlists.Include(p => p.User);
            return View(await melodyContext.ToListAsync());
        }

        // POST: Playlists/Create
        [HttpPost]
        public async Task<JsonResult> Create(string Name) // Changed from playlistName to Name
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Json(new { success = false, message = "Please provide a playlist name." });
            }

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            // Check if the playlist already exists for this user
            bool exists = await _context.Playlists
                .AnyAsync(p => p.Name.Equals(Name, StringComparison.OrdinalIgnoreCase) && p.UserId == userId); // Use Name here

            if (exists)
            {
                return Json(new { success = false, message = "A playlist with this name already exists." });
            }

            // Create and save the playlist
            var playlist = new Playlist { Name = Name, UserId = userId };
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Playlist created successfully!" });
        }

    }
}
