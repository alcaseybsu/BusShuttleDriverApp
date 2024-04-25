using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusShuttleDriver.Web.ViewModels
{
    public class StartViewModel
    {
        public int SelectedBusId { get; set; }
        public int SelectedLoopId { get; set; }
        public SelectList? AvailableBuses { get; set; }
        public SelectList? AvailableLoops { get; set; }
    }
}
