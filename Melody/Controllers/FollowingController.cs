using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Microsoft.AspNetCore.Identity;

namespace Melody.Controllers
{
    public class FollowingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAntiforgery _antiforgery;
        private readonly MelodyContext _context;

        public FollowingController(MelodyContext context,IAntiforgery antiforgery,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _antiforgery = antiforgery;
            _userManager = userManager;
        }

        // GET: Following
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Following.Include(f => f.Artist).Include(f => f.User);
            return View(await melodyContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] Following model)
        {
            var userId = model.UserId;
            var artistId = model.ArtistId;
            var existingFollow = await _context.Following
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ArtistId == artistId);

            if (existingFollow != null)
            {
                // Unfollow the artist (remove from the database)
                _context.Following.Remove(existingFollow);
                await _context.SaveChangesAsync();

                return Json(new { isFollowing = false });
            }
            else
            {
                // Follow the artist (add to the database)
                var follow = new Following
                {
                    UserId = userId,
                    ArtistId = artistId
                };
                _context.Following.Add(follow);
                await _context.SaveChangesAsync();

                return Json(new { isFollowing = true });
            }
        }





        // GET: Following/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var following = await _context.Following
                .Include(f => f.Artist)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.FollowingId == id);
            if (following == null)
            {
                return NotFound();
            }

            return View(following);
        }

        // POST: Following/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var following = await _context.Following.FindAsync(id);
            if (following != null)
            {
                _context.Following.Remove(following);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowingExists(int id)
        {
            return _context.Following.Any(e => e.FollowingId == id);
        }
    }
}
