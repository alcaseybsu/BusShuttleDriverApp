using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteDetailsViewModel
    {
        public int Id { get; set; }
        public string? RouteName { get; set; }
        public string? LoopName { get; set; }
        public List<StopViewModel>? Stops { get; set; }
    }
}
