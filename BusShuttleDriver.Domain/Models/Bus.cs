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
    public class Bus
    {
        [Key]
        public int BusId { get; set; }

        [Required]
        public string? BusNumber { get; set; }

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }

        [ForeignKey("Session")]
        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}
