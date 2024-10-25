using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;

namespace Melody.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MelodyContext _context;

        public ArtistsController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.ToListAsync());
        }

        // GET: Artists/Details/5
        //[SubscriptionAuthorize("Free", "Bronze", "Silver", "Gold")]
        public async Task<IActionResult> Artist(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a=>a.Songs)
                .Include(s=>s.Albums)
                .Include(f => f.Followers)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

    }
}
