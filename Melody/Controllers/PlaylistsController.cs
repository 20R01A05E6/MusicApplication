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
        public JsonResult Create([FromBody] dynamic data)
        {
            string playlistName = data.name;

            if (!string.IsNullOrEmpty(playlistName))
            {
                // Check if the playlist name already exists (case-insensitive)
                var existingPlaylist = _context.Playlists
                    .FirstOrDefault(p => p.Name.Equals(playlistName, StringComparison.OrdinalIgnoreCase));

                if (existingPlaylist != null)
                {
                    return Json(new { success = false }); // Playlist name already exists
                }

                // Add the new playlist to the database
                var newPlaylist = new Playlist { Name = playlistName };
                _context.Playlists.Add(newPlaylist);
                _context.SaveChanges();

                return Json(new { success = true }); // Successful creation
            }

            return Json(new { success = false }); // Name is empty or invalid
        }

    }
}
