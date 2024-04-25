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
                .Include(r => r.RouteStops)
                .ThenInclude(rs => rs.Stop)
                .Select(r => new RouteViewModel
                {
                    Id = r.RouteId,
                    RouteName = r.RouteName,
                    LoopName = r.Loop.LoopName,
                    Stops = r
                        .RouteStops.OrderBy(rs => rs.Order)
                        .Select(rs => new StopViewModel
                        {
                            Name = rs.Stop.Name,
                            Latitude = rs.Stop.Latitude,
                            Longitude = rs.Stop.Longitude
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
            var loopSelectList = new SelectList(_context.Loops, "Id", "LoopName");
            var stopSelectList = new SelectList(_context.Stops, "Id", "Name").ToList(); // Explicitly convert SelectList to List<SelectListItem>
            return View(
                new RouteCreateViewModel
                {
                    AvailableLoops = loopSelectList,
                    AvailableStops = stopSelectList
                }
            );
        }

        // POST: Route/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(RouteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var newRoute = new Route
            {
                RouteName = viewModel.RouteName,
                LoopId = viewModel.SelectedLoopId
            };
            _context.Add(newRoute);
            await _context.SaveChangesAsync();

            foreach (var stopId in viewModel.SelectedStopIds)
            {
                _context.RouteStops.Add(
                    new RouteStop
                    {
                        RouteId = newRoute.RouteId,
                        StopId = stopId,
                        Order = viewModel.OrderedStopIds.IndexOf(stopId) + 1
                    }
                );
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Route/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var route = await _context
                .Routes.Include(r => r.Loop)
                .FirstOrDefaultAsync(r => r.RouteId == id);
            if (route == null)
            {
                _logger.LogWarning("Route not found: {RouteId}", id);
                return NotFound();
            }

            var viewModel = new RouteEditViewModel
            {
                Id = route.RouteId,
                RouteName = route.RouteName,
                SelectedLoopId = route.LoopId,
                AvailableLoops = new SelectList(_context.Loops, "Id", "LoopName", route.LoopId),
                SelectedStopIds = route.RouteStops.Select(rs => rs.StopId).ToList(),
                AvailableStops = new SelectList(_context.Stops, "Id", "Name")
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
                _logger.LogWarning("Mismatch ID on edit: {RouteId}", id);
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                _logger.LogError("Route not found during edit: {RouteId}", id);
                return NotFound();
            }

            route.RouteName = viewModel.RouteName;
            route.LoopId = viewModel.SelectedLoopId;
            _context.Update(route);
            await _context.SaveChangesAsync();

            // Update stops associated with the route
            var currentStops = _context.RouteStops.Where(rs => rs.RouteId == id).ToList();
            _context.RouteStops.RemoveRange(currentStops);

            foreach (var stopId in viewModel.SelectedStopIds)
            {
                _context.RouteStops.Add(new RouteStop { RouteId = id, StopId = stopId });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Route/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Route deleted: {RouteId}", id);
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
