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
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Foreign key to Bus
        [Required]
        public int BusId { get; set; }
        public Bus? Bus { get; set; }

        // Foreign key to Loop
        [Required]
        public int LoopId { get; set; }
        public Loop? Loop { get; set; }

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
    }
}
