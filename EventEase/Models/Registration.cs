using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Registration
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Attendance flag (false until checked in)
        public bool Attended { get; set; } = false;
    }
}
