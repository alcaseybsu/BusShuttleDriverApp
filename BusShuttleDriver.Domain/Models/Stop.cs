using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Domain.Models
{
    public class Stop
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string? Name { get; set; } = string.Empty;

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public int Order { get; set; }

        public int? LoopId { get; set; } // Make nullable

        public virtual Loop? Loop { get; set; }
    }
}
