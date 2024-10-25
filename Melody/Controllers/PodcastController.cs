using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Melody.Filters;

namespace Melody.Controllers
{
    public class PodcastController : Controller
    {
        private readonly MelodyContext _context;

        public PodcastController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Podcast
        [SubscriptionAuthorize("Gold","Silver")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Podcasts.ToListAsync());
        }

        // GET: Podcast/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Podcast/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PodcastId,Name,Host,EpisodesCount,CreatedAt")] Podcast podcast)
        {
            if (ModelState.IsValid)
            {
                _context.Add(podcast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(podcast);
        }
        [SubscriptionAuthorize("Gold")]
        public async Task<IActionResult> Podcast(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var podcast = await _context.Podcasts
                .FirstOrDefaultAsync(p => p.PodcastId == id);
            if (podcast == null)
            {
                return NotFound();
            }

            return View(podcast);
        }



        private bool PodcastExists(int id)
        {
            return _context.Podcasts.Any(e => e.PodcastId == id);
        }
    }
}
