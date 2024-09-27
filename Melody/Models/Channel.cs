using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Melody.Models
{
    public class Channel
    {
        [Key]
        public int ChannelId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<ChannelPodcasts> ChannelPodcasts { get; set; }
    }
}
