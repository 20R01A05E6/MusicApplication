using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
namespace Melody.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly MelodyContext _context;

        public PlaylistController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Playlists
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Playlists.Include(p => p.UserDetails);
            return View(await melodyContext.ToListAsync());
        }

        // GET: Playlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlists = await _context.Playlists
                .Include(p => p.UserDetails)
                .FirstOrDefaultAsync(m => m.PlaylistId == id);
            if (playlists == null)
            {
                return NotFound();
            }

            return View(playlists);
        }

        // GET: Playlists/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email");
            return View();
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistId,Name,UserId,CreatedAt")] Playlists playlists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playlists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", playlists.UserId);
            return View(playlists);
        }

        // GET: Playlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlists = await _context.Playlists.FindAsync(id);
            if (playlists == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", playlists.UserId);
            return View(playlists);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaylistId,Name,UserId,CreatedAt")] Playlists playlists)
        {
            if (id != playlists.PlaylistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playlists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaylistsExists(playlists.PlaylistId))
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
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "Email", playlists.UserId);
            return View(playlists);
        }

        // GET: Playlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlists = await _context.Playlists
                .Include(p => p.UserDetails)
                .FirstOrDefaultAsync(m => m.PlaylistId == id);
            if (playlists == null)
            {
                return NotFound();
            }

            return View(playlists);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playlists = await _context.Playlists.FindAsync(id);
            if (playlists != null)
            {
                _context.Playlists.Remove(playlists);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaylistsExists(int id)
        {
            return _context.Playlists.Any(e => e.PlaylistId == id);
        }
    }
}
