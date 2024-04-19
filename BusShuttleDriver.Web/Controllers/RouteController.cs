using Microsoft.AspNetCore.Mvc;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BusShuttleDriver.Web.Controllers
{
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
            var routes = await _context.Routes
                .Include(r => r.Loop)
                .Include(r => r.Bus) // Ensure the Bus navigation property is properly configured in Route model
                .Select(r => new RouteViewModel
                {
                    Id = r.Id,
                    RouteName = r.RouteName,
                    BusNumber = r.Bus.BusNumber.ToString(),
                    LoopName = r.Loop.Name,
                    StopsCount = r.Stops.Count,
                    Stops = r.Stops.OrderBy(s => s.Order).Select(s => new StopViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude
                    }).ToList(),
                }).ToListAsync();

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
                    LoopId = model.SelectedLoopId,
                };

                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown data if validation fails
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
                route.Order = model.Order;
                route.BusId = model.SelectedBusId;
                route.LoopId = model.SelectedLoopId;

                _context.Update(route);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, reload dropdown data and return the Edit view
            model.AvailableBuses = GetBusesSelectList();
            model.AvailableLoops = GetLoopsSelectList();
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

            var route = await _context.Routes
                                      .Include(r => r.Stops)
                                      .FirstOrDefaultAsync(m => m.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            var model = new EditStopsViewModel
            {
                RouteId = route.Id,
                RouteName = route.RouteName,
                Stops = route.Stops.OrderBy(s => s.Order).Select(s => new StopViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order
                }).ToList()
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
                var route = await _context.Routes.Include(r => r.Stops).FirstOrDefaultAsync(r => r.Id == id);
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



        // Utility methods to get dropdown data
        private List<SelectListItem> GetBusesSelectList()
        {
            return _context.Buses.Select(b => new SelectListItem { Value = b.BusId.ToString(), Text = b.BusNumber.ToString() }).ToList();
        }

        private List<SelectListItem> GetLoopsSelectList()
        {
            return _context.Loops.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Name }).ToList();
        }

        private List<SelectListItem> GetStopsSelectList()
        {
            return _context.Stops.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }).ToList();
        }
    }
}
