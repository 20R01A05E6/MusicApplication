using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
namespace Melody.Controllers
{
    public class LikedController : Controller
    {
        private readonly MelodyContext _context;

        public LikedController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Likeds
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Liked.Include(l => l.Song).Include(l => l.UserDetails);
            return View(await melodyContext.ToListAsync());
        }

        // GET: Likeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liked = await _context.Liked
                .Include(l => l.Song)
                .Include(l => l.UserDetails)
                .FirstOrDefaultAsync(m => m.LikedId == id);
            if (liked == null)
            {
                return NotFound();
            }

            return View(liked);
        }

        // GET: Likeds/Create
        public IActionResult Create()
        {
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "ArtistName");
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email");
            return View();
        }

        // POST: Likeds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikedId,UserId,SongId,CreatedAt")] Liked liked)
        {
            if (ModelState.IsValid)
            {
                _context.Add(liked);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "ArtistName", liked.SongId);
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", liked.UserId);
            return View(liked);
        }

        // GET: Likeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liked = await _context.Liked.FindAsync(id);
            if (liked == null)
            {
                return NotFound();
            }
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "ArtistName", liked.SongId);
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", liked.UserId);
            return View(liked);
        }

        // POST: Likeds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikedId,UserId,SongId,CreatedAt")] Liked liked)
        {
            if (id != liked.LikedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(liked);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikedExists(liked.LikedId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SongId"] = new SelectList(_context.Song, "SongId", "ArtistName", liked.SongId);
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", liked.UserId);
            return View(liked);
        }

        // GET: Likeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var liked = await _context.Liked
                .Include(l => l.Song)
                .Include(l => l.UserDetails)
                .FirstOrDefaultAsync(m => m.LikedId == id);
            if (liked == null)
            {
                return NotFound();
            }

            return View(liked);
        }

        // POST: Likeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var liked = await _context.Liked.FindAsync(id);
            if (liked != null)
            {
                _context.Liked.Remove(liked);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikedExists(int id)
        {
            return _context.Liked.Any(e => e.LikedId == id);
        }
    }
}
