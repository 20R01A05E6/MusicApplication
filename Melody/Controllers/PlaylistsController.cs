using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Melody.Filters;
using Microsoft.AspNetCore.Authorization;

namespace Melody.Controllers
{
    [Authorize]
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
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var userPlaylists = _context.Playlists
                                        .Include(p => p.User)
                                        .Where(p => p.UserId == userId.Value)
                                        .ToListAsync();

            return View(await userPlaylists);
        }


        [HttpPost]
        [Route("api/Playlists")]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist model)
        {
            // Retrieve UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Signup");
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


        public async Task<IActionResult> Playlist(int id)
        {
            // Retrieve UserId from session
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Load the playlist with songs, ensuring it belongs to the logged-in user
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .ThenInclude(s => s.Album)  // Eager load Album data
                .Where(p => p.UserId == userId.Value && p.PlaylistId == id)
                .FirstOrDefaultAsync();


            if (playlist == null)
            {
                return NotFound("Playlist not found or you do not have permission to access it.");
            }

            return View(playlist);
        }


        [HttpGet]
        public async Task<IActionResult> AddSongs(int playlistId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            var playlist = await _context.Playlists
                                         .Where(p => p.UserId == userId.Value && p.PlaylistId == playlistId)
                                         .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound("Playlist not found or you do not have permission to access it.");
            }

            var existingSongIds = _context.PlaylistSongs
                                          .Where(ps => ps.PlaylistId == playlistId)
                                          .Select(ps => ps.SongId)
                                          .ToList();

            var songs = await _context.Songs
                                      .Include(s => s.Album)  // Include the Album information
                                      .Where(s => !existingSongIds.Contains(s.SongId))
                                      .ToListAsync();

            ViewBag.PlaylistId = playlistId;
            return View(songs);
        }


        /*[HttpPost]
        public async Task<IActionResult> SaveSongsToPlaylist(int playlistId, List<int> songIds)
        {
            // Get the current user's ID
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Get the playlist to verify it belongs to the current user
            var playlist = await _context.Playlists
                                         .Where(p => p.UserId == userId.Value && p.PlaylistId == playlistId)
                                         .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound("Playlist not found or you do not have permission to access it.");
            }

            // Retrieve existing songs in the playlist to avoid duplicates
            var existingSongIds = _context.PlaylistSongs
                                          .Where(ps => ps.PlaylistId == playlistId)
                                          .Select(ps => ps.SongId)
                                          .ToList();

            // Add selected songs that are not already in the playlist
            foreach (var songId in songIds)
            {
                if (!existingSongIds.Contains(songId))  // Check if the song is already in the playlist
                {
                    var playlistSong = new PlaylistSong
                    {
                        PlaylistId = playlistId,
                        SongId = songId,
                        UserId = userId.Value  // Assign the UserId
                    };
                    _context.PlaylistSongs.Add(playlistSong);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Playlist", new { id = playlistId });
        }
*/

        [HttpPost]
        public async Task<IActionResult> SaveSongsToPlaylist(int playlistId, List<int> songIds)
        {
            // Get the current user's ID
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Get the playlist to verify it belongs to the current user
            var playlist = await _context.Playlists
                                         .Where(p => p.UserId == userId.Value && p.PlaylistId == playlistId)
                                         .FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound("Playlist not found or you do not have permission to access it.");
            }

            // Retrieve existing songs in the playlist to avoid duplicates
            var existingSongIds = _context.PlaylistSongs
                                          .Where(ps => ps.PlaylistId == playlistId)
                                          .Select(ps => ps.SongId)
                                          .ToList();

            // Track if a new song is added
            bool isNewSongAdded = false;

            // Add selected songs that are not already in the playlist
            foreach (var songId in songIds)
            {
                if (!existingSongIds.Contains(songId)) // Check if the song is already in the playlist
                {
                    var playlistSong = new PlaylistSong
                    {
                        PlaylistId = playlistId,
                        SongId = songId,
                        UserId = userId.Value // Assign the UserId
                    };
                    _context.PlaylistSongs.Add(playlistSong);
                    isNewSongAdded = true;

                    // Set cover image if it's the first song being added
                    if (string.IsNullOrEmpty(playlist.CoverImagePath) || playlist.CoverImagePath == "default-image.png")
                    {
                        var song = await _context.Songs.Include(s => s.Album)
                                                        .FirstOrDefaultAsync(s => s.SongId == songId);

                        if (song?.Album?.ImagePath != null)
                        {
                            playlist.CoverImagePath = song.Album.ImagePath;
                        }
                    }
                }
            }

            if (isNewSongAdded)
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Playlist", new { id = playlistId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSelectedSongs(int playlistId, [FromBody] List<int> songIds)
        {
            // Validate if songIds is null or empty
            if (songIds == null || songIds.Count == 0)
            {
                return BadRequest("No songs selected for removal.");
            }

            // Log for debugging: Check incoming data
            Console.WriteLine($"Playlist ID: {playlistId}, Song IDs: {string.Join(", ", songIds)}");

            // Retrieve the songs in the playlist that need to be removed
            var playlistSongs = _context.PlaylistSongs
                                         .Where(ps => ps.PlaylistId == playlistId && songIds.Contains(ps.SongId))
                                         .ToList();

            if (playlistSongs.Any())
            {
                _context.PlaylistSongs.RemoveRange(playlistSongs);
                await _context.SaveChangesAsync();
                return Ok(); // Return success response
            }

            return NotFound("Songs not found in the playlist.");
        }



        /*[HttpGet]
        public IActionResult Library()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            // Get the user by email
            var user = _context.UserDetails.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Fetch all playlists created by the user
            var playlists = _context.Playlists
                                    .Where(p => p.UserId == user.UserId)
                                    .OrderByDescending(p => p.PlaylistId) // Display the newest playlists first
                                    .ToList();

            // Pass the playlists to the view
            return View(playlists);
        }*/
    }
}
