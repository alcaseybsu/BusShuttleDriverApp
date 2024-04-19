using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Stop
    {
        public int Id { get; set; } // Primary key

        [Required(ErrorMessage = "Stop name is required.")]
        public string Name { get; set; } = string.Empty; // Ensure never null

        [Required(ErrorMessage = "Latitude is required.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        public double Longitude { get; set; }

        public int? RouteId { get; set; } // Foreign key for Route
        public RouteModel Route { get; set; } = new RouteModel(); // Initialize to avoid null reference

        public int Order { get; set; } // Order of the stop in the route
    }





}