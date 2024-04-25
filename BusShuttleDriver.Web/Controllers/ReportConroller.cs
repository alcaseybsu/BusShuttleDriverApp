using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Data;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string dateInputHourly)
        {
            var model = new ReportViewModel
            {
                LoopDropdown = await _context
                    .Loops.Select(l => new SelectListItem
                    {
                        Text = l.LoopName,
                        Value = l.Id.ToString()
                    })
                    .ToListAsync(),
                EntryDate = dateInputHourly ?? DateTime.Today.ToString("MM-dd-yyyy")
            };

            if (!string.IsNullOrEmpty(dateInputHourly))
            {
                DateTime date = DateTime.Parse(dateInputHourly);
                model.LoopReports = await GenerateLoopReports(date);
            }

            return View(model);
        }

        private async Task<List<LoopReport>> GenerateLoopReports(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var entries = await _context
                .Entries.Where(e => e.Timestamp >= startOfDay && e.Timestamp < endOfDay)
                .ToListAsync();

            var groupedData = entries
                .GroupBy(e => new
                {
                    LoopId = e.Id,
                    Interval = new DateTime(
                        e.Timestamp.Year,
                        e.Timestamp.Month,
                        e.Timestamp.Day,
                        e.Timestamp.Hour,
                        e.Timestamp.Minute / 15 * 15,
                        0
                    )
                })
                .Select(g => new
                {
                    LoopId = g.Key.LoopId,
                    Interval = g.Key.Interval,
                    Boarded = g.Sum(e => e.Boarded),
                    LeftBehind = g.Sum(e => e.LeftBehind)
                })
                .OrderBy(g => g.LoopId)
                .ThenBy(g => g.Interval)
                .ToList();

            var loops = await _context.Loops.ToListAsync();
            var loopReports = new List<LoopReport>();

            foreach (var loop in loops)
            {
                var reports = groupedData
                    .Where(g => g.LoopId == loop.Id)
                    .Select(g => new IntervalReport
                    {
                        Interval = g.Interval.ToString("HH:mm"),
                        Boarded = g.Boarded
                    })
                    .ToList();

                loopReports.Add(
                    new LoopReport { LoopName = loop.LoopName, IntervalReports = reports }
                );
            }

            return loopReports;
        }
    }
}
