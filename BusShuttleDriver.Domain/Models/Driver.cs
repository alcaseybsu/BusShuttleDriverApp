using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BusShuttleDriver.Domain.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int AccountId { get; set; } // Foreign key for Account

        // Link to the currently active RouteSession
        public int? ActiveRouteSessionId { get; set; }
        public RouteSession ActiveRouteSession { get; set; }

    }
}
