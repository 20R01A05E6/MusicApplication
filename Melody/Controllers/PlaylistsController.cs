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

        [HttpPost]
        [Route("api/Playlists")]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist model)
        {
            // Retrieve UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User is not logged in." });
            }

            if (string.IsNullOrEmpty(model.Name) || model.Name.Length > 100)
            {
                return BadRequest(new { success = false, message = "Invalid playlist name." });
            }

            // Create a new playlist object
            var newPlaylist = new Playlist
            {
                Name = model.Name,
                UserId = userId.Value,  // Assign the UserId from the session
            };

            // Save the new playlist to the database
            _context.Playlists.Add(newPlaylist);
            await _context.SaveChangesAsync();

            // Return success response with the new playlist ID
            return Ok(new { success = true, playlistId = newPlaylist.PlaylistId });
        }

        public IActionResult Playlist()
        {
            return View();
        }

    }
}
