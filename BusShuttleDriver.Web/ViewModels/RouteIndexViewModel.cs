using System.ComponentModel.DataAnnotations;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Web.ViewModels
{
    public class RouteIndexViewModel
    {
        public IEnumerable<RouteModel> Routes { get; set; } = new List<RouteModel>();
        public RouteViewModel NewRoute { get; set; } = new RouteViewModel();

        public StopViewModel NewStop { get; set; } = new StopViewModel();
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AvailableLoops { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
    }
}
