using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteCreateModel
    {
        public int Id { get; set; } // Primary key

        [Required(ErrorMessage = "The route name is required.")]
        [Display(Name = "Route Name")]
        public string? RouteName { get; set; }

        [Required(ErrorMessage = "Selecting a loop is required.")]
        [Display(Name = "Loop")]
        public int? SelectedLoopId { get; set; }

        public List<SelectListItem> AvailableLoops { get; set; } = new List<SelectListItem>();

        [Display(Name = "Stops")]
        public List<int> SelectedStopIds { get; set; } = new List<int>();

        public List<SelectListItem> AvailableStops { get; set; } = new List<SelectListItem>(); // Display stops as options

        public string OrderedStopIds { get; set; } = ""; // Stores ordered stop IDs as a comma-separated string

        public RouteCreateModel()
        {
            AvailableLoops = new List<SelectListItem>();
            AvailableStops = new List<SelectListItem>();
            SelectedStopIds = new List<int>();
            OrderedStopIds = "";
        }
    }
}
