using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class PodcastEpisodes
    {
        [Key]
        public int EpisodeId { get; set; }

        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        // Navigation Property
        public Podcast Podcast { get; set; }
    }
}
