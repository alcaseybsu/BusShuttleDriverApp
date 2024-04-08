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
            return View(await _context.Routes.ToListAsync());
        }

        // GET: Routes/Create
        public IActionResult Create()
        {
            var viewModel = new RouteViewModel
            {
                Buses = GetBusesSelectList(), // Assume this method fetches buses and converts them into SelectListItems
                Loops = GetLoopsSelectList() // Assume this method fetches loops and converts them into SelectListItems
            };

            return View(viewModel);
        }

        private IEnumerable<SelectListItem> GetLoopsSelectList()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<SelectListItem> GetBusesSelectList()
        {
            throw new NotImplementedException();
        }


        // POST: Routes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteId,Color")] Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(route);
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