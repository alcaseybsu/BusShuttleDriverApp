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

        public async Task<IActionResult> Edit(int? id)
        {
            var route = await _context.Routes.FindAsync(id);
            var model = new RouteCreateModel
            {
                Id = route.Id,
                RouteName = route.RouteName,
                Order = route.Order,
                SelectedBusId = route.BusId,
                SelectedLoopId = route.LoopId,
                AvailableBuses = await _context.Buses.Select(b => new SelectListItem
                {
                    Value = b.BusId.ToString(),
                    Text = b.BusNumber.ToString()
                }).ToListAsync(),
                AvailableLoops = await _context.Loops.Select(l => new SelectListItem
                {
                    Text = l.Name,
                    Value = l.Id.ToString()
                }).ToListAsync()
            };
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
