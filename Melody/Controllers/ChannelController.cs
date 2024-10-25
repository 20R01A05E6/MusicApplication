using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Melody.Data;
using Melody.Models;
using Melody.Filters;
/*using Melody.Authorization;
*/
namespace Melody.Controllers
{
    public class ChannelController : Controller
    {
        private readonly MelodyContext _context;

        public ChannelController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Channel
        [SubscriptionAuthorize("Gold")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Channels.ToListAsync());
        }

        // GET: Channel/Create
        public IActionResult Create()
        {
            return View();
        }

    }
}
