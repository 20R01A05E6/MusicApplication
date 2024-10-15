using Melody.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class PlaylistSong
    {
        [Key]
        public int PlaylistSongId { get; set; }

        [ForeignKey("Playlist")]
        public int PlaylistId { get; set; }

        [ForeignKey("Song")]
        public int SongId { get; set; }

        // Relationships
        public Playlist Playlist { get; set; }
        public Song Song { get; set; }
    }
}
