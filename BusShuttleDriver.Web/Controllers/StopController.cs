using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class StopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StopController>? _logger;

        public StopController(ApplicationDbContext context, ILogger<StopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Stops
        public async Task<IActionResult> Index()
        {
            var stops = await _context.Stops.Include(s => s.Loop).ToListAsync();
            var stopViewModels = stops
                .Select(s => new StopViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    LoopName = s.Loop?.LoopName ?? "No Loop Assigned"
                })
                .ToList();

            return View(stopViewModels);
        }

        // GET: Create Stop
        public async Task<IActionResult> Create()
        {
            var model = new StopViewModel
            {
                AvailableLoops = new SelectList(await _context.Loops.ToListAsync(), "Id", "Name")
            };
            return View(model);
        }

        // POST: Create Stop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StopViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Only validate loop if a loop is selected
                if (model.SelectedLoopId.HasValue && model.SelectedLoopId.Value > 0)
                {
                    var isValidLoop = await _context.Loops.AnyAsync(l =>
                        l.Id == model.SelectedLoopId
                    );
                    if (!isValidLoop)
                    {
                        ModelState.AddModelError("", "Selected loop is invalid.");
                        model.AvailableLoops = new SelectList(
                            await _context.Loops.ToListAsync(),
                            "Id",
                            "Name",
                            model.SelectedLoopId
                        );
                        return View(model);
                    }
                }

                var stop = new Stop
                {
                    Name = model.Name,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    LoopId =
                        model.SelectedLoopId.HasValue && model.SelectedLoopId.Value > 0
                            ? model.SelectedLoopId
                            : null
                };

                _context.Add(stop);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger?.LogError(ex, "Unable to save changes: {Message}", ex.Message);
                    ModelState.AddModelError(
                        "",
                        "Unable to save changes. Try again, and if the problem persists see your system administrator."
                    );
                }
            }

            // Re-populate the dropdown list in case of return to the form due to error or invalid ModelState
            model.AvailableLoops = new SelectList(
                await _context.Loops.ToListAsync(),
                "Id",
                "Name",
                model.SelectedLoopId
            );
            return View(model);
        }

        // POST: Stops/UpdateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(List<StopOrderUpdateViewModel> updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest("No updates provided.");
            }

            foreach (var update in updates)
            {
                var stop = await _context.Stops.FindAsync(update.Id);
                if (stop != null)
                {
                    stop.Order = update.NewOrder;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ViewModel to hold updates
        public class StopOrderUpdateViewModel
        {
            public int Id { get; set; }
            public int NewOrder { get; set; }
        }

        // GET: Stops/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            var viewModel = new StopViewModel
            {
                Id = stop.Id,
                Name = stop.Name,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                SelectedLoopId = stop.LoopId,
                AvailableLoops = new SelectList(
                    await _context.Loops.ToListAsync(),
                    "Id",
                    "Name",
                    stop.LoopId
                )
            };

            return View(viewModel);
        }

        // POST: Stops/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StopViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var stop = await _context.Stops.FindAsync(id);
                if (stop == null)
                {
                    return NotFound();
                }

                stop.Name = viewModel.Name;
                stop.Latitude = viewModel.Latitude;
                stop.Longitude = viewModel.Longitude;
                stop.LoopId = viewModel.SelectedLoopId;

                _context.Update(stop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableLoops = new SelectList(
                await _context.Loops.ToListAsync(),
                "Id",
                "Name",
                viewModel.SelectedLoopId
            );
            return View(viewModel);
        }

        // GET: Stop/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            _context.Stops.Remove(stop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
