using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class ChannelPodcasts
    {
        [Key]
        public int ChannelPodcastId { get; set; }

        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        [ForeignKey("Podcast")]
        public int PodcastId { get; set; }

        // Navigation Properties
        public Channel Channel { get; set; }
        public Podcast Podcast { get; set; }
    }
}
