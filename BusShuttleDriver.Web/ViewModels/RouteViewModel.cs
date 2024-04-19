using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        public string? RouteName { get; set; }
        public string? BusNumber { get; set; }
        public string? LoopName { get; set; }
        public int StopsCount { get; set; }
        public List<StopViewModel> Stops { get; set; }
        public int Order { get; set; }

        public RouteViewModel()
        {
            Stops = new List<StopViewModel>();
        }
    }

}
