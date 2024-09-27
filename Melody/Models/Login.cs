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

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime LastLogin { get; set; }

        // Navigation Property
        public UserDetails UserDetails { get; set; }
    }
}
