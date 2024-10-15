using System.ComponentModel.DataAnnotations;
namespace Melody.Models
{
    public class Channel
    {
        [Key]
        public int ChannelId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        // Relationships
        public ICollection<ChannelPodcast> ChannelPodcasts { get; set; }
    }
}
