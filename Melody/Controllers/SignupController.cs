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
    public class SignupController : Controller
    {
        private readonly MelodyContext _context;

        public SignupController(MelodyContext context)
        {
            _context = context;
        }

        // GET: Signup
        public async Task<IActionResult> Index()
        {
            var melodyContext = _context.Signups.Include(s => s.User);
            return View(await melodyContext.ToListAsync());
        }
    }
}
