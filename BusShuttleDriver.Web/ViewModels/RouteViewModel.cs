using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteViewModel
    {
        public int Id { get; set; }

        public string? LoopName { get; set; }
        public int StopsCount { get; set; }
        public int? SelectedLoopId { get; set; }
        public List<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();
        public List<StopViewModel> Stops { get; set; } = new List<StopViewModel>();
        public string RouteName { get; set; }
        public List<int> SelectedStopIds { get; set; } = new List<int>();
        public List<SelectListItem> AvailableStops { get; set; } = new List<SelectListItem>();

        public RouteViewModel()
        {
            RouteName = string.Empty;
            AvailableStops = new List<SelectListItem>();
            AvailableLoops = new List<SelectListItem>();
            SelectedStopIds = new List<int>();
        }
    }
}
