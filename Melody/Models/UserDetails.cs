using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class UserDetails
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Lastname { get; set; }

        [MaxLength(15)]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        // Relationships
        public Signup Signup { get; set; }
        public ICollection<Liked> LikedSongs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Following> Followings { get; set; }
        public ICollection<Login> Logins { get; set; }
    }
}
