using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        public string CoverImagePath { get; set; } = "default-image.png"; // Default cover image path
        
        // Relationships
        public UserDetails User { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}