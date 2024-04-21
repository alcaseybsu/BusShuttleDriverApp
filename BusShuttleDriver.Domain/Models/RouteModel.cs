using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Domain.Models
{
    public class RouteModel
    {
        public int Id { get; set; } // Primary key

        [Display(Name = "Route Name")]
        public string? RouteName { get; set; }

        public int? LoopId { get; set; } // Foreign key for Loop

        public Loop Loop { get; set; } = new Loop(); // Initialize to avoid null ref

        public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}
