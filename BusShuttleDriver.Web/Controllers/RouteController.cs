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
    [Authorize(Roles = "Manager")]
    public class RouteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RouteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Routes
        public async Task<IActionResult> Index()
        {
            var routes = await _context
                .Routes.Include(r => r.Loop)
                .Include(r => r.Bus)
                .Select(r => new RouteViewModel
                {
                    Id = r.Id,
                    RouteName = r.RouteName,
                    BusNumber = r.Bus.BusNumber.ToString(),
                    LoopName = r.Loop.Name,
                    Order = r.Order,
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

        // GET: Routes/Create
        public IActionResult Create()
        {
            var model = new RouteCreateModel
            {
                AvailableBuses = GetBusesSelectList(),
                AvailableLoops = GetLoopsSelectList(),
                AvailableStops = GetStopsSelectList()
            };
            return View(model);
        }

        // POST: Routes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var route = new RouteModel
                {
                    RouteName = model.RouteName,
                    BusId = model.SelectedBusId,
                    LoopId = model.SelectedLoopId
                };

                // Add the route first to generate its ID
                _context.Routes.Add(route);
                await _context.SaveChangesAsync(); // Save changes to generate route.Id

                // Parse OrderedStopIds and maintain the correct order
                var orderedStopIds = model
                    .OrderedStopIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                // Fetch stops in the order they are supposed to be set
                var stopEntities = _context
                    .Stops.Where(s => orderedStopIds.Contains(s.Id))
                    .ToList()
                    .OrderBy(s => orderedStopIds.IndexOf(s.Id)); // Order by the sequence in orderedStopIds

                foreach (var stop in stopEntities)
                {
                    stop.RouteId = route.Id; // Set RouteId to the newly created route's ID
                    route.Stops.Add(stop); // Add stop to the route's Stops collection
                }

                // Save changes again to update stops with the new RouteId
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload necessary data
            model.AvailableBuses = GetBusesSelectList();
            model.AvailableLoops = GetLoopsSelectList();
            model.AvailableStops = GetStopsSelectList();
            return View(model);
        }

        // POST: Routes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RouteCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var route = await _context.Routes.FindAsync(model.Id);
                if (route == null)
                {
                    return NotFound();
                }

                route.RouteName = model.RouteName;
                route.BusId = model.SelectedBusId;
                route.LoopId = model.SelectedLoopId;

                if (model.OrderedStopIds != null)
                {
                    var orderedStopIds = model
                        .OrderedStopIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToList();
                    var stopEntities = _context
                        .Stops.Where(s => orderedStopIds.Contains(s.Id))
                        .ToList();

                    foreach (var stopId in orderedStopIds)
                    {
                        var stop = stopEntities.FirstOrDefault(s => s.Id == stopId);
                        if (stop != null)
                        {
                            stop.RouteId = route.Id; // Just to ensure it's correctly associated, not necessary if it's already set
                        }
                    }
                }

                _context.Update(route);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, reload dropdown data and return the Edit view
            model.AvailableBuses = GetBusesSelectList();
            model.AvailableLoops = GetLoopsSelectList();
            model.AvailableStops = GetStopsSelectList();
            return View(model);
        }

        // DELETE: Route/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }

            // Disassociate stops from the route
            foreach (var stop in route.Stops)
            {
                stop.RouteId = null;
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Route/EditStops/5
        public async Task<IActionResult> EditStops(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context
                .Routes.Include(r => r.Stops)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            var model = new EditStopsViewModel
            {
                RouteId = route.Id,
                RouteName = route.RouteName,
                Stops = route
                    .Stops.OrderBy(s => s.Order)
                    .Select(s => new StopViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Order = s.Order
                    })
                    .ToList()
            };

            return View(model);
        }

        // POST: Route/EditStops/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStops(int id, EditStopsViewModel model)
        {
            if (id != model.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var route = await _context
                    .Routes.Include(r => r.Stops)
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (route == null)
                {
                    return NotFound();
                }

                // Update stop orders based on the model
                foreach (var stopModel in model.Stops)
                {
                    var stop = route.Stops.FirstOrDefault(s => s.Id == stopModel.Id);
                    if (stop != null)
                    {
                        stop.Order = stopModel.Order;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context
                .Routes.Include(r => r.Stops)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route); // Pass route to the view
        }

        // POST: Entry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> StartRoute(StartRouteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var driverId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current logged-in driver's ID
                    if (driverId == null)
                        return NotFound("Driver not found.");

                    var driver = await _context.Drivers.FindAsync(int.Parse(driverId)); // Retrieve the driver entity from the database
                    if (driver == null)
                        return NotFound("Driver not found.");

                    // Check if driver has an active route session
                    if (driver.ActiveRouteSessionId != null)
                    {
                        ModelState.AddModelError("", "You already have an active route session.");
                        return View("Error", ModelState);
                    }

                    var routeSession = new RouteSession
                    {
                        BusId = model.SelectedBusId,
                        LoopId = model.SelectedLoopId,
                        StartTime = DateTime.Now,
                        DriverId = driver.DriverId,
                        IsActive = true
                    };

                    _context.RouteSessions.Add(routeSession);
                    driver.ActiveRouteSessionId = routeSession.Id; // Set the active session ID
                    await _context.SaveChangesAsync(); // Asynchronously save changes to the database

                    // Redirect to Entry create view with session details
                    return RedirectToAction(
                        "Create",
                        "Entry",
                        new { routeSessionId = routeSession.Id }
                    );
                }
                catch (Exception ex)
                {
                    // Log the error and display a message
                    ModelState.AddModelError("", "Error starting route: " + ex.Message);
                }
            }

            // Repopulate dropdowns if validation or save error
            model.AvailableBuses = GetBusesSelectList();
            model.AvailableLoops = GetLoopsSelectList();
            return View("DriverIndex", model);
        }

        public async Task<IActionResult> EndRoute(int routeSessionId)
        {
            var routeSession = await _context.RouteSessions.FindAsync(routeSessionId);
            if (routeSession == null)
                return NotFound("Route session not found.");

            var driver = await _context.Drivers.FindAsync(routeSession.DriverId);
            if (driver != null)
            {
                driver.ActiveRouteSessionId = null; // Clear the active session
            }

            _context.RouteSessions.Remove(routeSession);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Driver");
        }

        [Authorize(Roles = "Driver")]
        public IActionResult DriverIndex()
        {
            var model = new StartRouteViewModel
            {
                AvailableBuses = GetBusesSelectList(),
                AvailableLoops = GetLoopsSelectList()
            };
            return View(model);
        }

        private List<SelectListItem> GetBusesSelectList()
        {
            return _context
                .Buses.Select(b => new SelectListItem
                {
                    Value = b.BusId.ToString(),
                    Text = b.BusNumber.ToString()
                })
                .ToList();
        }

        private List<SelectListItem> GetLoopsSelectList()
        {
            return _context
                .Loops.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name })
                .ToList();
        }

        private List<SelectListItem> GetStopsSelectList()
        {
            return _context
                .Stops.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
        }
    }
}
