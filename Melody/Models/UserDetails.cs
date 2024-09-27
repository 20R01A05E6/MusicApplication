using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class UserDetails
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Firstname { get; set; }

        [Required, StringLength(50)]
        public string Lastname { get; set; }

        [Required, Phone]
        public string Mobile { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public ICollection<Playlists> Playlists { get; set; }
        public ICollection<Liked> LikedSongs { get; set; }
        public Login Login { get; set; }
    }
}
