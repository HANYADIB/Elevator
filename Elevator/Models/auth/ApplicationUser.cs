using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Elevator.Models.auth
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? LandLine { get; set; }

        [MaxLength(100)]
        public string? NationalId { get; set; }

        [MaxLength(200)]
        public string? AddressArea { get; set; }

        [MaxLength(200)]
        public string? Street { get; set; }

        public int? BuildingNumber { get; set; }

        public int? FloorNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Age { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public string? ImagePath { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool Active { get; set; }
    }
}
