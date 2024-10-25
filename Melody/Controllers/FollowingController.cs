using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Melody.Filters;

namespace Melody.Controllers
{
    [SubscriptionAuthorize("Silver","Gold")]
    public class FollowingController : Controller
    {
        private readonly MelodyContext _context;
        public FollowingController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Following
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Signup");
            }

            // Query only the records where UserId matches the current user's ID
            var followingList = await _context.Following
                .Where(f => f.UserId == userId.Value)
                .Include(f => f.Artist)
                .ToListAsync();

            return View(followingList);
        }

        public IActionResult ToggleFollow([FromBody] ToggleFollowRequest request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in" });
            }

            int artistId = request.ArtistId;

            var artist = _context.Artists.Include(a => a.Followers).FirstOrDefault(a => a.ArtistId == artistId);
            if (artist == null)
            {
                return NotFound(new { success = false, message = "Artist not found" });
            }

            var existingFollow = _context.Following.FirstOrDefault(f => f.UserId == userId && f.ArtistId == artistId);
            bool isFollowing;

            if (existingFollow != null)
            {
                // Unfollow
                _context.Following.Remove(existingFollow);
                isFollowing = false;
            }
            else
            {
                // Follow
                var newFollow = new Following { UserId = userId.Value, ArtistId = artistId };
                _context.Following.Add(newFollow);
                isFollowing = true;
            }

            _context.SaveChanges();

            return Json(new { success = true, following = isFollowing });
        }

    }
}
