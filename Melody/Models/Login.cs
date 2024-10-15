using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime LastLogin { get; set; }

        // Relationships
        public UserDetails User { get; set; }
    }
}
