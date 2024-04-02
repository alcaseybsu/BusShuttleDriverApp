using Microsoft.AspNetCore.Mvc;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BusShuttleDriver.Web.Controllers
{
    public class LoopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Loops.ToListAsync());
        }

        // GET: Loops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoopId,Name")] Loop loop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loop);
        }

        // GET: Loops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loop = await _context.Loops.FindAsync(id);
            if (loop == null)
            {
                return NotFound();
            }
            return View(loop);
        }
    }
}