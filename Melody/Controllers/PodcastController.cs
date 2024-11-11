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

        [SubscriptionAuthorize("Gold", "Silver")]
        public IActionResult Podcast(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            return View();
        }
    }
}
