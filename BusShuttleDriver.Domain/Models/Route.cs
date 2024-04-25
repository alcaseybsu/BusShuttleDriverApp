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

        [Required]
        public string? RouteName { get; set; }

        [Required]
        public int LoopId { get; set; }
        public virtual Loop? Loop { get; set; }
        public virtual ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}
