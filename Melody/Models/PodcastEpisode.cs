using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class PodcastEpisode
    {
        [Key]
        public int EpisodeId { get; set; }

        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(10)]
        public string Duration { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        // Relationships
        public Podcast Podcast { get; set; }
    }
}
