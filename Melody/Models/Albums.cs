using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Albums
    {
        [Key]
        public int AlbumId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string ArtistName { get; set; }

        public DateTime ReleaseDate { get; set; }

        // Navigation Property
        public ICollection<Song> Songs { get; set; }
    }
}
