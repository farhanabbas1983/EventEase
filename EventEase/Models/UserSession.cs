using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class UserSession
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime SignedInAt { get; set; } = DateTime.UtcNow;
    }
}
