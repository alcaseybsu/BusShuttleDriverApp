using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                var loop = new Loop { LoopName = viewModel.LoopName };
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

        // GET: RouteIndex
        public async Task<IActionResult> RouteIndex()
        {
            var loops = await _context
                .Loops.Include(l => l.Stops)
                .Select(l => new RouteViewModel
                {
                    Id = l.Id,
                    LoopName = l.LoopName,
                    Stops = l
                        .Stops.Select(s => new StopViewModel
                        {
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude
                        })
                        .ToList(),
                    RouteName = l.LoopName
                })
                .ToListAsync();

            return View("RouteIndex", loops);
        }

        // GET: Loop/RouteCreate
        public async Task<IActionResult> CreateRoute()
        {
            var viewModel = new RouteCreateViewModel
            {
                AvailableLoops = new SelectList(
                    await _context.Loops.ToListAsync(),
                    "Id",
                    "LoopName"
                ),
                AvailableStops = new SelectList(await _context.Stops.ToListAsync(), "Id", "Name")
            };
            return View(viewModel);
        }

        // POST: Loop/RouteCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoute(RouteCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var loop = await _context
                    .Loops.Include(l => l.Stops)
                    .FirstOrDefaultAsync(l => l.Id == viewModel.SelectedLoopId);
                if (loop == null)
                {
                    ModelState.AddModelError("", "The selected loop does not exist.");
                    return View(viewModel);
                }

                // Reset the order for all stops in the loop
                foreach (var stop in loop.Stops)
                {
                    stop.Order = int.MaxValue; // Set a high order value to ensure it goes to the end if not included in the new order list
                }

                // Update the order of stops based on the provided list
                int order = 1;
                foreach (var stopId in viewModel.OrderedStopIds)
                {
                    var stop = loop.Stops.FirstOrDefault(s => s.Id == stopId);
                    if (stop != null)
                    {
                        stop.Order = order++;
                    }
                }

                // Save the updated order
                await _context.SaveChangesAsync();
                return RedirectToAction("RouteIndex"); // Assuming RouteIndex is the view that lists all "routes"
            }

            // Re-populate the AvailableLoops if returning to the form
            viewModel.AvailableLoops = new SelectList(
                await _context.Loops.ToListAsync(),
                "Id",
                "LoopName",
                viewModel.SelectedLoopId
            );
            return View(viewModel);
        }
    }
}
