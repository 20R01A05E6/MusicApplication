using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Playlists
    {
        [Key]
        public int PlaylistId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public UserDetails UserDetails { get; set; }
        public ICollection<PlaylistSongs> PlaylistSongs { get; set; }
    }
}
