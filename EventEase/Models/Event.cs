using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Event name must be 100 characters or fewer")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location must be 200 characters or fewer")]
        public string Location { get; set; } = string.Empty;

        // Registrations for this event (in-memory list for now)
        public List<Registration> Registrations { get; set; } = new();
    }
}
