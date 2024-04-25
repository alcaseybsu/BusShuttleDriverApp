using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusShuttleDriver.Domain.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Loop")]
        public string? LoopName { get; set; }

        [Display(Name = "Stop")]
        public string? StopName { get; set; }

        [Display(Name = "Date & Time")]
        public DateTime Timestamp { get; set; }

        [Display(Name = "Boarded")]
        [Required(ErrorMessage = "Number Boarded is required")]
        public int Boarded { get; set; }

        [Display(Name = "Left Behind")]
        [Required(ErrorMessage = "Number Left Behind is required")]
        public int LeftBehind { get; set; }
    }
}
