using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;

namespace Melody.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MelodyContext _context;

        public AlbumsController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Albums.Include(a => a.Artist);
            return View(await melodyContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Album(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }
    }
}
