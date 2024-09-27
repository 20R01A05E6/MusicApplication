using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class PlaylistSongs
    {
        [Key]
        public int PlaylistSongId { get; set; }

        [ForeignKey("Playlists")]
        public int PlaylistId { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }

        // Navigation Properties
        public Playlists Playlists { get; set; }
        public Song Song { get; set; }
    }
}
