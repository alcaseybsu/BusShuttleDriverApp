using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Web.ViewModels
{
    public class EntryViewModel
    {
        public int RouteSessionId { get; set; }
        public int SelectedStopId { get; set; }
        public int Boarded { get; set; }
        public int LeftBehind { get; set; }
        public DateTime Timestamp { get; set; }

        // Dropdown list of stops for the route
        public SelectList AvailableStops { get; set; }

        public EntryViewModel()
        {
            Timestamp = DateTime.UtcNow; // Init timestamp
        }
    }
}


