using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Liked
    {
        [Key]
        public int LikedId { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public UserDetails UserDetails { get; set; }
        public Song Song { get; set; }
    }
}
