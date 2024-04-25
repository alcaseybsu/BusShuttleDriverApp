using System;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusShuttleDriver.Web.Controllers
{
    [Authorize(Roles = "Driver")]
    public class EntryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EntryController> _logger;

        public EntryController(ApplicationDbContext context, ILogger<EntryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Entry/Index
        public async Task<IActionResult> Index()
        {
            var entries = await _context
                .Entries.Select(e => new EntryViewModel
                {
                    StopName = e.StopName,
                    Timestamp = e.Timestamp,
                    Boarded = e.Boarded,
                    LeftBehind = e.LeftBehind
                })
                .ToListAsync();

            return View(entries);
        }

        // GET: Start Session
        public IActionResult Start()
        {
            try
            {
                var buses = _context.Buses.ToList();
                var loops = _context.Loops.ToList();

                if (!buses.Any())
                {
                    TempData["Error"] = "No buses available. Please contact your manager.";
                    return RedirectToAction(nameof(Index));
                }

                if (!loops.Any())
                {
                    TempData["Error"] = "No loops available. Please contact your manager.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new StartViewModel
                {
                    AvailableLoops = new SelectList(loops, "Id", "LoopName"),
                    AvailableBuses = new SelectList(buses, "BusId", "BusNumber")
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load data in Start method.");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: Start Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(StartViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Start", model);
            }

            var session = new Session
            {
                BusId = model.SelectedBusId,
                LoopId = model.SelectedLoopId,
                Timestamp = DateTime.Now,
                IsActive = true
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create", "Entry", new { sessionId = session.Id });
        }

        // GET: Create Entry View for Drivers
        public async Task<IActionResult> Create(int sessionId)
        {
            var session = await _context
                .Sessions.Include(s => s.Loop)
                .ThenInclude(l => l.Stops)
                .SingleOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
            {
                TempData["Error"] = "Session not found. Please contact your manager.";
                return RedirectToAction("Start", "Entry");
            }

            if (session.Loop == null)
            {
                TempData["Error"] = "No Loops available. Please contact your manager.";
                return RedirectToAction("Start", "Entry");
            }

            var stops = session.Loop.Stops.OrderBy(s => s.Order).ToList();
            if (stops == null || !stops.Any())
            {
                TempData["Error"] =
                    "No stops are found on this loop. Try another Loop or contact your manager.";
                return RedirectToAction("Start", "Entry", new { id = session.LoopId });
            }

            var model = new EntryViewModel
            {
                SessionId = sessionId,
                AvailableStops = new SelectList(stops, "Id", "Name")
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
                var entry = new Entry
                {
                    StopName = model.StopName,
                    Boarded = model.Boarded,
                    LeftBehind = model.LeftBehind,
                    Timestamp = DateTime.UtcNow
                };

                _context.Entries.Add(entry);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Create), new { sessionId = model.SessionId });
            }

            var session = await _context
                .Sessions.Include(s => s.Loop)
                .FirstOrDefaultAsync(s => s.Id == model.SessionId);

            var stops = session?.Loop?.Stops.OrderBy(s => s.Order).ToList();
            if (stops != null)
            {
                model.AvailableStops = new SelectList(stops, "Id", "Name", model.SelectedStopId);
            }

            return View(model);
        }

        // Method to End a Session
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndSession(int sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session != null)
            {
                session.IsActive = false;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Driver");
            }
            return NotFound("Session not found.");
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
