using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
namespace Melody.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [ForeignKey("Artist")]
        public int ArtistId { get; set; }

        public string ImagePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public int? somecol { get; set; }   

        // Relationships
        public Artist Artist { get; set; }
        public ICollection<Song> Songs { get; set; }

    }
}
