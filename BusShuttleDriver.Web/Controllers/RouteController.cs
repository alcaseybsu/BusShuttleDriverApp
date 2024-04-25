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
    [Authorize]
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
        {
            _context = context;
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
                    RouteId = r.RouteId,
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
            // Populate SelectList for loops
            var loopSelectList = new SelectList(_context.Loops, "Id", "LoopName");
            return View(new RouteCreateViewModel { AvailableLoops = loopSelectList });
        }

        // POST: Route/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(RouteCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var loop = await _context.Loops.FindAsync(viewModel.SelectedLoopId);
                if (loop == null)
                {
                    ModelState.AddModelError("", "Selected loop does not exist.");
                    return View(viewModel);
                }

                var newRoute = new RouteModel
                {
                    LoopId = loop.Id,
                    RouteName = $"Route {(_context.Routes.Count() + 1)}"
                };

                _context.Add(newRouteModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
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
                Id = route.Id,
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
                    Id = r.Id,
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
