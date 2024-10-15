using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Signup
    {
        [Key]
        public int SignupId { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)] // Ensures it is treated as a password input in forms
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match the password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        // Relationships
        public UserDetails User { get; set; }
    }
}
