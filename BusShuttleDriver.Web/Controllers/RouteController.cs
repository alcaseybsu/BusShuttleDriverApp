using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Route/Index
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var routes = await _context
                .Routes.Include(r => r.Loop)
                .Include(r => r.Stops)
                .Select(r => new RouteViewModel
                {
                    Id = r.Id,
                    RouteName = r.RouteName,
                    LoopName = r.Loop.Name,
                    StopsCount = r.Stops.Count,
                    Stops = r
                        .Stops.OrderBy(s => s.Order)
                        .Select(s => new StopViewModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Latitude = s.Latitude,
                            Longitude = s.Longitude
                        })
                        .ToList(),
                })
                .ToListAsync();

            return View(routes);
        }

        // GET: Route/Create
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new RouteCreateModel
            {
                AvailableLoops = GetLoopsSelectList(),
                AvailableStops = GetStopsSelectList()
            };
            return View(model);
        }

        // POST: Route/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(RouteCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var route = new RouteModel
                {
                    RouteName = model.RouteName,
                    LoopId = model.SelectedLoopId
                };

                _context.Routes.Add(route);
                await _context.SaveChangesAsync(); // Save to generate route.Id

                // Assuming UpdateStopOrder now correctly expects a List<int> for the first argument
                await UpdateStopOrder(model.SelectedStopIds, route.Id); // Handle Stops

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown data if the ModelState is not valid
            model.AvailableLoops = GetLoopsSelectList();
            model.AvailableStops = GetStopsSelectList();
            return View(model);
        }

        // POST: Route/UpdateStopOrder
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateStopOrder(List<int> stopIds, int routeId)
        {
            var stops = await _context
                .Stops.Where(s => s.RouteId == routeId && stopIds.Contains(s.Id))
                .ToListAsync();
            for (int i = 0; i < stopIds.Count; i++)
            {
                var stop = stops.FirstOrDefault(s => s.Id == stopIds[i]);
                if (stop != null)
                {
                    stop.Order = i;
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // Helper method to get stops on a loop (via its route)
        [Authorize(Roles = "Manager,Driver")] // Allow drivers to view stops
        public async Task<IActionResult> GetStopsOnLoop(int loopId)
        {
            var route = await _context
                .Routes.Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.LoopId == loopId);

            if (route == null)
                return NotFound("Route not found for the selected loop.");

            var routeViewModel = new RouteViewModel
            {
                RouteName = route.RouteName,
                Stops = route
                    .Stops.OrderBy(s => s.Order)
                    .Select(s => new StopViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Order = s.Order,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude
                    })
                    .ToList()
            };

            return View("DriverView", routeViewModel); // Assuming 'DriverView' is the view where drivers interact with route data
        }

        // Helper method to update route stops
        [Authorize(Roles = "Manager")]
        private void UpdateRouteStops(string orderedStopIds, int routeId)
        {
            if (string.IsNullOrEmpty(orderedStopIds))
                return;

            var orderedIds = orderedStopIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            var stops = _context.Stops.Where(s => orderedIds.Contains(s.Id)).ToList();

            foreach (var stop in stops)
            {
                stop.RouteId = routeId;
                stop.Order = orderedIds.IndexOf(stop.Id);
            }

            _context.SaveChanges();
        }

        // GET: Route/Edit/5
        [HttpGet, ActionName("Edit")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            var model = new RouteCreateModel
            {
                Id = route.Id,
                RouteName = route.RouteName,
                SelectedLoopId = route.LoopId,
                AvailableLoops = GetLoopsSelectList(),
                AvailableStops = GetStopsSelectList()
            };

            return View(model);
        }

        // POST: Routes/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")] // Only managers can edit routes
        public async Task<IActionResult> Edit(RouteCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableLoops = GetLoopsSelectList();
                model.AvailableStops = GetStopsSelectList();
                return View(model);
            }

            var route = await _context.Routes.FindAsync(model.Id);
            if (route == null)
            {
                return NotFound();
            }

            route.RouteName = model.RouteName;
            route.LoopId = model.SelectedLoopId;

            _context.Update(route);
            await _context.SaveChangesAsync();

            UpdateRouteStops(model.OrderedStopIds, route.Id);

            return RedirectToAction(nameof(Index));
        }

        // DELETE: Route/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")] // Only managers can delete routes
        public async Task<IActionResult> Delete(int? id)
        {
            var route = await _context
                .Routes.Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (route == null)
            {
                return NotFound();
            }

            // Check if this is the only route associated with the loop
            bool isLastRouteForLoop = !_context.Routes.Any(r =>
                r.LoopId == route.LoopId && r.Id != id
            );

            foreach (var stop in route.Stops)
            {
                stop.RouteId = null; // Detach stops from the route
            }

            // Delete route
            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper methods to get select list items
        [Authorize(Roles = "Manager,Driver")]
        private List<SelectListItem> GetLoopsSelectList()
        {
            return _context
                .Loops.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToList();
        }

        [Authorize(Roles = "Manager,Driver")]
        private List<SelectListItem> GetStopsSelectList()
        {
            return _context
                .Stops.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
        }
    }
}
