using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;

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
        public async Task<IActionResult> Artist(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a=>a.Songs)
                .Include(s=>s.Albums)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

    }
}
