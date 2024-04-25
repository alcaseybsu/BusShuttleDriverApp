using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Routing;

namespace BusShuttleDriver.Domain.Models
{
    public class LoopReport
    {
        [Required]
        public string? LoopName { get; set; }

        [Required]
        public int[] HourlyBoarded { get; set; } = new int[24];

        [Required]
        public int[] HourlyLeftBehind { get; set; } = new int[24];
        public List<IntervalReport>? IntervalReports { get; set; }
    }
}
