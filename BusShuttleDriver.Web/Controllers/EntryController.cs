using Microsoft.AspNetCore.Mvc;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Web.Controllers
{
    public class EntryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create Entry View for Drivers - Shows dropdown for current/first stop
        public async Task<IActionResult> Create(int routeSessionId)
        {
            var routeSession = await _context.RouteSessions
                .Include(rs => rs.Loop)
                .ThenInclude(l => l.Stops) // Assuming Loop has Stops navigation property
                .FirstOrDefaultAsync(rs => rs.Id == routeSessionId);

            if (routeSession == null || routeSession.Loop == null)
            {
                return NotFound("Route session or loop not found.");
            }

            var model = new EntryViewModel
            {
                RouteSessionId = routeSessionId,
                AvailableStops = new SelectList(routeSession.Loop.Stops, "Id", "Name")
            };

            return View(model);
        }

        // POST: Entry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var entry = new Entry
                    {
                        Id = model.SelectedStopId,
                        Boarded = model.Boarded,
                        LeftBehind = model.LeftBehind,
                        Timestamp = DateTime.UtcNow  // Optional: record time of entry
                    };
                    _context.Entries.Add(entry);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Create), new { routeSessionId = model.RouteSessionId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating entry: " + ex.Message);
                }
            }

            // Reload stops if there's an error
            var routeSession = await _context.RouteSessions
                .Include(rs => rs.Loop)
                .FirstOrDefaultAsync(rs => rs.Id == model.RouteSessionId);
            if (routeSession?.Loop != null)
            {
                model.AvailableStops = new SelectList(routeSession.Loop.Stops, "Id", "Name", model.SelectedStopId);
            }
            return View(model);
        }

        // Method to End a Route Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndRoute(int routeSessionId)
        {
            var routeSession = await _context.RouteSessions.FindAsync(routeSessionId);
            if (routeSession != null)
            {
                routeSession.IsActive = false; // Mark the session as inactive
                routeSession.EndTime = DateTime.Now; // Optionally record the end time
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Driver"); // Redirect to driver's main page
            }
            return NotFound("Route session not found.");
        }

        // GET: Entries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }
            return View(entry);
        }

        // POST: Entries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Entry entry)
        {
            if (id != entry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(entry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entry);
        }

        // GET: Entries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

