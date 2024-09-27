using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Podcast
    {
        [Key]
        public int PodcastId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Host { get; set; }

        public int EpisodesCount { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<PodcastEpisodes> PodcastEpisodes { get; set; }
    }
}
