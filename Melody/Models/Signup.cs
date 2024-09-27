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

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public UserDetails UserDetails { get; set; }
    }
}
