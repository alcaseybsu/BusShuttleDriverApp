using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Domain.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; } // Foreign key for Account

        // Link to the currently active RouteSession
        [ForeignKey("RouteSession")]
        public int? ActiveSessionId { get; set; }
        public Session? ActiveSession { get; set; }
    }
}
