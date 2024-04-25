using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteCreateViewModel
    {
        [Key]
        public int RouteId { get; set; }

        [Required(ErrorMessage = "Enter a route name.")]
        [Display(Name = "Route Name")]
        public string RouteName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Select a loop.")]
        [Display(Name = "Select Loop")]
        public int SelectedLoopId { get; set; }

        public SelectList? AvailableLoops { get; set; }

        public List<SelectListItem>? AvailableStops { get; set; }
        public List<int>? SelectedStopIds { get; set; }

        // IDs of stops in order in route
        public List<int> OrderedStopIds { get; set; } = new List<int>();

        // Details of stops displayed on RouteCreate view
        public List<StopViewModel>? Stops { get; set; }
    }
}
