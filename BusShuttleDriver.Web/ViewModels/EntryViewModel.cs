using System.ComponentModel.DataAnnotations;
using NuGet.Packaging.Signing;

namespace BusShuttleDriver.Web.ViewModels
{
    public class EntryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Date & TIme")]
        public DateTime Timestamp { get; set; }
        public int Boarded { get; set; }

        [Display(Name = "Left Behind")]
        public int LeftBehind { get; set; }

    }
}


