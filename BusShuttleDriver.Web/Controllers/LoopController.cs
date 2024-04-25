using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LoopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loop/Index
        public async Task<IActionResult> Index()
        {
            var loops = await _context
                .Loops.Include(l => l.Stops)
                .Select(loop => new LoopViewModel
                {
                    Id = loop.Id,
                    LoopName = loop.LoopName,
                    StopsCount = loop.Stops.Count,
                    Stops = loop
                        .Stops.OrderBy(s => s.Order)
                        .Select(s => new StopViewModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude
                        })
                        .ToList()
                })
                .ToListAsync();

            return View(loops);
        }

        // GET: Loop/Create
        public IActionResult Create()
        {
            var model = new LoopViewModel();
            return View(model);
        }

        // POST: Loop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoopViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(viewModel.LoopName))
                {
                    ModelState.AddModelError("LoopName", "Loop name is required.");
                    return View(viewModel);
                }

                var loop = new Loop(viewModel.LoopName);
                _context.Add(loop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Loop/Edit/{id}
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

            var viewModel = new LoopViewModel { Id = loop.Id, LoopName = loop.LoopName };
            return View(viewModel);
        }

        // POST: Loop/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoopViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var loop = await _context.Loops.FindAsync(id);
                if (loop == null)
                {
                    return NotFound();
                }

                loop.LoopName = viewModel.LoopName;
                _context.Update(loop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // DELETE: Loop/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Loop ID not specified.");
            }

            var loop = await _context
                .Loops.Include(l => l.Stops)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (loop == null)
            {
                return NotFound("Loop not found.");
            }

            if (loop.Stops.Any())
            {
                return BadRequest("Loop cannot be deleted because it has associated stops.");
            }

            _context.Loops.Remove(loop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
