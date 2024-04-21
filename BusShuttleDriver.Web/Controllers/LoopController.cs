using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class LoopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Loop/Index
        [HttpGet("Loop/Index")]
        public async Task<IActionResult> Index()
        {
            // Fetch all loops
            var loops = await _context.Loops.ToListAsync();

            // Convert the data to LoopViewModel
            var loopViewModels = loops
                .Select(loop => new LoopViewModel
                {
                    Id = loop.Id,
                    Name = loop.Name,
                    // You might want to include information whether the loop has associated routes with stops
                    HasActiveRoutes = loop.Routes.Any(r => r.Stops.Any())
                })
                .ToList();

            return View(loopViewModels); // Return the list of all loops with additional info
        }

        // GET: Loop/Create
        [HttpGet("Loop/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loop/Create
        [HttpPost("Loop/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoopViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel == null)
                {
                    throw new ArgumentNullException(nameof(viewModel));
                }

                if (viewModel.Name == null)
                {
                    throw new ArgumentException("ViewModel.Name is null", nameof(viewModel));
                }

                var loop = new Loop { Name = viewModel.Name };

                _context.Add(loop);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Loop/Edit/{id}
        [HttpGet("Loop/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loop = await _context.Loops.FindAsync(id);
            if (loop == null)
            {
                return NotFound();
            }

            var viewModel = new LoopViewModel { Id = loop.Id, Name = loop.Name };

            return View(viewModel);
        }

        // POST: Loop/Edit/{id}
        [HttpPost("Loop/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoopViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var loop = await _context.Loops.FindAsync(id);
                if (loop == null)
                {
                    return NotFound();
                }

                if (viewModel.Name == null)
                {
                    throw new ArgumentException("ViewModel.Name is null", nameof(viewModel));
                }

                loop.Name = viewModel.Name;

                _context.Update(loop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Loop/Delete/{id}
        [HttpGet("Loop/Delete/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Loop ID not specified.");
            }

            var loop = await _context
                .Loops.Include(l => l.Routes)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (loop == null)
            {
                return NotFound("Loop not found.");
            }

            try
            {
                // If the loop is associated with any routes, disassociate them
                foreach (var route in loop.Routes)
                {
                    route.LoopId = null; // Disassociate the loop from the route
                }

                _context.Loops.Remove(loop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(
                    "Error",
                    new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        Message = "An error occurred while deleting the loop: " + ex.Message
                    }
                );
            }
        }
    }
}
