using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Domain.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }

        public string? RouteName { get; set; }

        public int LoopId { get; set; }
        public Loop? Loop { get; set; }

        public virtual ICollection<RouteStop>? RouteStops { get; set; }
    }
}
