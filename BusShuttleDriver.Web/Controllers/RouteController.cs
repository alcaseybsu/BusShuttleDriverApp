using Microsoft.AspNetCore.Mvc;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var viewModel = new RouteIndexViewModel
            {
                Routes = await _context.Routes.Include(r => r.Loops).ToListAsync(),
                AvailableLoops = _context.Loops.Select(l => new SelectListItem { Value = l.LoopId.ToString(), Text = l.Name })
            };

            return View(viewModel);
        }



        // GET: Routes/Create
        public IActionResult Create()
        {
            var viewModel = new RouteViewModel
            {
                Buses = GetBusesSelectList(),
                Loops = GetLoopsSelectList()
            };

            return View(viewModel);
        }

        private IEnumerable<SelectListItem> GetLoopsSelectList()
        {
            return _context.Loops.Select(l => new SelectListItem
            {
                Value = l.LoopId.ToString(),
                Text = l.Name
            }).ToList();
        }

        private IEnumerable<SelectListItem> GetBusesSelectList()
        {
            return _context.Buses.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BusNumber.ToString()
            }).ToList();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteViewModel routeViewModel)
        {
            if (ModelState.IsValid)
            {
                var route = new RouteModel
                {
                    RouteName = routeViewModel.RouteName,
                    Order = routeViewModel.Order,
                    // Assuming you have logic to associate loops and buses
                };
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate dropdowns if returning to form
            routeViewModel.Buses = GetBusesSelectList();
            routeViewModel.Loops = GetLoopsSelectList();

            return RedirectToAction(nameof(Index));
        }

        // GET: Routes/Edit/5
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
            return View(route);
        }
    }
}