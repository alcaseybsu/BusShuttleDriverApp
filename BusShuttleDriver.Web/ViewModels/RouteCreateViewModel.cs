using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Select a loop.")]
        [Display(Name = "Select Loop")]
        public int SelectedLoopId { get; set; }

        public SelectList AvailableLoops { get; set; }

        public SelectList AvailableStops { get; set; }
        public List<int> OrderedStopIds { get; set; } = new List<int>();
        public List<StopViewModel>? Stops { get; set; }

        // Constructor
        public RouteCreateViewModel()
        {
            var loops = GetLoops();
            AvailableLoops = new SelectList(loops, "Value", "Text");
        }

        private List<SelectListItem> GetLoops()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Loop 1" },
                new SelectListItem { Value = "2", Text = "Loop 2" },
                // Add more loops as necessary
            };
        }
    }
}
