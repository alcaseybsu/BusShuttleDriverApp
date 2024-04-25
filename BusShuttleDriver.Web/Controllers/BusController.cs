using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class BusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Buses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Buses.ToListAsync());
        }

        // GET: Buses/Create
        public async Task<IActionResult> Create()
        {
            var model = new BusCreateViewModel
            {
                ExistingBuses = await _context.Buses.ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(BusCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model.NewBus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create)); // Redirect back to the form
            }
            // If we got this far, something failed; redisplay form
            model.ExistingBuses = _context.Buses.ToList(); // Reload existing buses to display
            return View(model);
        }

        // GET: Buses/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        // POST: Buses/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BusNumber")] Bus bus)
        {
            if (id != bus.BusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusExists(bus.BusId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Create));
            }
            return View(bus);
        }

        private bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.BusId == id);
        }

        // GET: Buses/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Create));
        }
    }
}
