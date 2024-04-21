using System.Collections.Generic;
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
    [Authorize(Roles = "Manager")]
    public class StopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stop/Index
        [HttpGet("Stop/Index")]
        public async Task<IActionResult> Index()
        {
            var stops = await _context
                .Stops.Select(s => new StopViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude
                })
                .ToListAsync();

            return View(stops);
        }

        // GET: Stop/Create
        public IActionResult Create()
        {
            return View(new StopViewModel());
        }

        // POST: Stop/Create
        [HttpPost("Stop/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StopViewModel viewModel)
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

                var stop = new Stop
                {
                    Name = viewModel.Name,
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

            if (viewModel.Name == null)
            {
                throw new ArgumentException("ViewModel.Name is null", nameof(viewModel));
            }

            stop.Name = viewModel.Name;
            stop.Latitude = viewModel.Latitude;
            stop.Longitude = viewModel.Longitude;

            _context.Update(stop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Stop/UpdateStopOrder
        [HttpPost]
        public async Task<IActionResult> UpdateStopOrder(int stopId, int newPosition)
        {
            var stop = await _context.Stops.FindAsync(stopId);
            if (stop == null)
            {
                return NotFound();
            }

            var currentOrder = stop.Order;
            var stopsToUpdate = await _context
                .Stops.Where(s => s.RouteId == stop.RouteId && s.Id != stopId)
                .ToListAsync();

            if (newPosition < currentOrder)
            {
                foreach (
                    var s in stopsToUpdate.Where(s =>
                        s.Order >= newPosition && s.Order < currentOrder
                    )
                )
                {
                    s.Order++;
                }
            }
            else
            {
                foreach (
                    var s in stopsToUpdate.Where(s =>
                        s.Order <= newPosition && s.Order > currentOrder
                    )
                )
                {
                    s.Order--;
                }
            }

            stop.Order = newPosition;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: Stop/Delete/{id}
        [HttpGet("Stop/Delete/{id}")]
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
