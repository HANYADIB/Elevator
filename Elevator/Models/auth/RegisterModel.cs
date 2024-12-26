using System.ComponentModel.DataAnnotations;

namespace Elevator.Models.auth
{
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string MiddleName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        public string? Email { get; set; }
        [Required, StringLength(100)]

        public string? PhoneNumber { get; set; }

        [Required, StringLength(256)]
        public string Password { get; set; }



        [MaxLength(100)]
        public string? NationalId { get; set; }

        [MaxLength(200)]
        public string? AddressArea { get; set; }

        [MaxLength(200)]
        public string? Street { get; set; }

        public int? BuildingNumber { get; set; }

        public int? FloorNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        [MaxLength(10)]
        public string? Age { get; set; }

      
        [MaxLength(50)]
        public string? LandLine { get; set; }
        public IFormFile? Image { get; set; }

      
    }
}
