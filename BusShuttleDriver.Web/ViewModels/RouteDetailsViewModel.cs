using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Route Name")]
        [Required(ErrorMessage = "Route name is required.")]
        public string RouteName { get; set; }

        [Display(Name = "Loop Name")]
        public string LoopName { get; set; }

        [Display(Name = "Stops")]
        public List<StopViewModel>? Stops { get; set; }
    }
}
