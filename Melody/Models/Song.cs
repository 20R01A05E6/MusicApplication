using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("Albums")]
        public int AlbumId { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required, StringLength(100)]
        public string ArtistName { get; set; }

        [Required, StringLength(50)]
        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        // Navigation Properties
        public Albums Albums { get; set; }
    }
}
