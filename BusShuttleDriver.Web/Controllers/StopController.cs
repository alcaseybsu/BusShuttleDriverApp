using Microsoft.AspNetCore.Mvc;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BusShuttleDriver.Web.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BusShuttleDriver.Web.Controllers
{
    public class StopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stops
        public async Task<IActionResult> Index()
        {
            var stops = await _context.Stops
                .Select(s => new StopViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude
                }).ToListAsync();

            return View(stops);
        }

        // GET: Stops/Create
        public IActionResult Create()
        {
            return View(new StopViewModel());
        }

        // POST: Stops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StopViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var stop = new Stop
                {
                    Name = viewModel?.Name,
                    Latitude = viewModel.Latitude,
                    Longitude = viewModel.Longitude
                };

                _context.Stops.Add(stop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Stops/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            var viewModel = new StopViewModel
            {
                Id = stop.Id,
                Name = stop.Name,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude
            };

            return View(viewModel);
        }

        // POST: Stops/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StopViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            stop.Name = viewModel?.Name;
            stop.Latitude = viewModel.Latitude;
            stop.Longitude = viewModel.Longitude;

            _context.Update(stop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Stops/Delete/{id}
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stop = await _context.Stops.FindAsync(id);
            if (stop == null)
            {
                return NotFound();
            }

            _context.Stops.Remove(stop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}