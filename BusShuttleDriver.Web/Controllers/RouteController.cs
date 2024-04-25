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
using Route = BusShuttleDriver.Domain.Models.Route;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize]
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RouteController> _logger;

        public RouteController(ApplicationDbContext context, ILogger<RouteController> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: Routes for Manager
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var routes = await _context
                .Routes.Include(r => r.Loop)
                .ThenInclude(l => l.Stops)
                .Select(r => new RouteViewModel
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    LoopName = r.Loop.LoopName,
                    Stops = r
                        .Loop.Stops.Select(s => new StopViewModel
                        {
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude
                        })
                        .ToList()
                })
                .ToListAsync();

            return View(routes);
        }

        // GET: Route/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            try
            {
                // Populate SelectList for loops
                var loopSelectList = new SelectList(_context.Loops, "Id", "LoopName");
                var stopSelectList = new SelectList(_context.Stops, "Id", "Name");
                var stopSelectItems = stopSelectList.ToList(); // Convert SelectList to List<SelectListItem>

                return View(
                    new RouteCreateViewModel
                    {
                        AvailableLoops = loopSelectList,
                        AvailableStops = stopSelectItems
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to load data for route creation form: {Error}",
                    ex.Message
                );
                return View("Error"); // Consider redirecting to a generic error page or display a friendly error message
            }
        }

        // POST: Route/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(RouteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Log the state of the model if it's invalid
                _logger.LogWarning(
                    "Model state is invalid. Errors: {ModelStateErrors}",
                    string.Join(
                        "; ",
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                    )
                );

                // Re-populate the SelectList if returning to the form
                viewModel.AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName");
                return View(viewModel);
            }

            var loop = await _context.Loops.FindAsync(viewModel.SelectedLoopId);
            if (loop == null)
            {
                _logger.LogError("Loop with ID {LoopId} not found.", viewModel.SelectedLoopId);

                ModelState.AddModelError("", "Selected loop does not exist.");
                viewModel.AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName");
                return View(viewModel);
            }

            var newRoute = new Route { LoopId = loop.Id, RouteName = viewModel.RouteName };
            _context.Routes.Add(newRoute);

            _logger.LogInformation(
                "Attempting to add new route to the database: {RouteName}",
                newRoute.RouteName
            );
            _logger.LogInformation(
                "New route entity state before saving: {State}",
                _context.Entry(newRoute).State
            );

            try
            {
                var changes = await _context.SaveChangesAsync();
                _logger.LogInformation(
                    "{Changes} changes saved to the database. Route added successfully.",
                    changes
                );

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    "Error saving route: {Error}. Exception: {ExceptionMessage}",
                    ex.ToString(),
                    ex.Message
                );
                ModelState.AddModelError("", "Failed to save the route. Please try again.");

                // Re-populate the SelectList for recovery from failure
                viewModel.AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "An unexpected error occurred while saving the route: {ExceptionMessage}",
                    ex.Message
                );
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                viewModel.AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName");
                return View(viewModel);
            }
        }

        // GET: Route/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context
                .Routes.Include(r => r.Loop)
                .FirstOrDefaultAsync(r => r.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            var viewModel = new RouteEditViewModel
            {
                Id = route.RouteId,
                SelectedLoopId = route.LoopId,
                AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName", route.LoopId)
            };

            return View(viewModel);
        }

        // POST: Route/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, RouteEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var route = await _context.Routes.FindAsync(id);
                if (route == null)
                {
                    return NotFound();
                }

                route.LoopId = viewModel.SelectedLoopId;
                _context.Update(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // POST: Route/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Driver/ViewRoute
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> ViewRoute(int routeId)
        {
            var route = await _context
                .Routes.Include(r => r.Loop)
                .ThenInclude(l => l.Stops)
                .Where(r => r.RouteId == routeId)
                .Select(r => new RouteViewModel
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    LoopName = r.Loop.LoopName,
                    Stops = r
                        .Loop.Stops.OrderBy(s => s.Order)
                        .Select(s => new StopViewModel
                        {
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (route == null)
            {
                return NotFound("Route not found.");
            }

            return View(route);
        }
    }
}
