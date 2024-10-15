using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [ForeignKey("Album")]
        public int AlbumId { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        [MaxLength(10)]
        public string Duration { get; set; }

        [MaxLength(100)]
        public string ArtistName { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "File path is required.")]
        [StringLength(200, ErrorMessage = "File path cannot be longer than 200 characters.")]
        public string FilePath { get; set; }

        // Relationships
        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
        public ICollection<Liked> LikedByUsers { get; set; }
    }
}
