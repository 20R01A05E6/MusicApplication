using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Melody.Models
{
    public class Following
    {
        [Key]
        public int FollowingId { get; set; }

        [ForeignKey("UserDetails")]
        public int UserId { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        public UserDetails User { get; set; }
        public Artist Artist { get; set; }
    }
}
