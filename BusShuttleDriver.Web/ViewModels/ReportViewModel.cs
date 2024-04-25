using System.Collections.Generic;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class ReportViewModel
    {
        public string? EntryDate { get; set; }

        public string? SelectedDate { get; set; }
        public List<SelectListItem>? LoopDropdown { get; set; }
        public List<LoopReport>? LoopReports { get; set; }
    }
}
