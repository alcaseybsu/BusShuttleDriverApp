using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Domain.Models
{
    namespace BusShuttleDriver.Domain.Models
    {
        public class RouteCreateModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Please enter a name for the route.")]
            [Display(Name = "Route Name")]
            public string? RouteName { get; set; }

            [Required(ErrorMessage = "Please select a loop.")]
            [Display(Name = "Select Loop")]
            public int SelectedLoopId { get; set; }

            [Display(Name = "Select Stops")]
            public List<int> SelectedStopIds { get; set; }

            public List<SelectListItem> AvailableLoops { get; set; }
            public List<SelectListItem> AvailableStops { get; set; }

            // Optionally, maintain the order of stops as a comma-separated string
            [Display(Name = "Order of Stops")]
            public string OrderedStopIds { get; set; }

            public RouteCreateModel()
            {
                AvailableLoops = new List<SelectListItem>();
                AvailableStops = new List<SelectListItem>();
                SelectedStopIds = new List<int>();
                OrderedStopIds = ""; // Initialize as empty to handle null-checks more gracefully
            }
        }
    }
}
