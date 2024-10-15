using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Genre { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(50)]
        public string ImagePath { get; set; }

        // Relationships
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<Following> Followers { get; set; }
    }

}
