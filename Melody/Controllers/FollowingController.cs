using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Melody.Controllers
{
    [Authorize]
    public class FollowingController : Controller
    {
        private readonly MelodyContext _context;
        public FollowingController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Following
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Following.Include(f => f.Artist).Include(f => f.User);
            return View(await melodyContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFollow([FromBody] int artistId)
        {
            // Assuming you have the UserId stored in session
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Check if the user is already following the artist
            var following = await _context.Following
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ArtistId == artistId);

            if (following != null)
            {
                // User is following, so unfollow
                _context.Following.Remove(following);
                await _context.SaveChangesAsync();
                return Json(new { status = "unfollowed" });
            }
            else
            {
                // User is not following, so follow
                var newFollowing = new Following
                {
                    UserId = userId,
                    ArtistId = artistId
                };
                _context.Following.Add(newFollowing);
                await _context.SaveChangesAsync();
                return Json(new { status = "followed" });
            }
        }


    }
}
