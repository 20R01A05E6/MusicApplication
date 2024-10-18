using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;

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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
