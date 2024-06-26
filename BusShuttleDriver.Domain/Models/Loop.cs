using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Routing;

namespace BusShuttleDriver.Domain.Models
{
    public class Loop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LoopName { get; set; }

        public virtual ICollection<Stop> Stops { get; set; } = new List<Stop>();

        // Constructor with required LoopName
        public Loop(string loopName)
        {
            LoopName = loopName ?? throw new ArgumentNullException(nameof(loopName));
        }
    }
}
