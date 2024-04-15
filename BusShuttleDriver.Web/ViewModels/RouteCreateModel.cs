using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteCreateModel
    {
        public int Id { get; set; } // Primary key

        [Required]
        [Display(Name = "Route Name")]
        public string? RouteName { get; set; }

        public int Order { get; set; }

        [Required]
        [Display(Name = "Bus")]
        public int SelectedBusId { get; set; }

        [Required]
        [Display(Name = "Loop")]
        public int SelectedLoopId { get; set; }

        public List<SelectListItem> AvailableBuses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableStops { get; set; } = new List<SelectListItem>();

        public List<int> SelectedStopIds { get; set; }

        public RouteCreateModel()
        {
            AvailableBuses = new List<SelectListItem>();
            AvailableLoops = new List<SelectListItem>();
            AvailableStops = new List<SelectListItem>();
            SelectedStopIds = new List<int>();
        }
    }
}