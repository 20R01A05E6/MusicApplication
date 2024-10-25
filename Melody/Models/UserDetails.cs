using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class UserDetails
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [MaxLength(30)]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength (30)]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string SubscriptionType { get; set; } = "Free";

        public string? ProfileImagePath { get; set; }

        // Relationships
        public ICollection<Liked> LikedSongs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Following> Followings { get; set; }
    }
}
