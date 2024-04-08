using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Entry
    {
        public int Id { get; set; }

        [Display(Name = "Date & Time")]
        public DateTime Timestamp { get; set; }
        public int Boarded { get; set; }

        [Display(Name = "Left Behind")]
        public int LeftBehind { get; set; }
    }
}