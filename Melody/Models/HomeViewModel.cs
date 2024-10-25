namespace Melody.Models
{
    public class HomeViewModel
    {
        public List<Artist> Artists { get; set; }
        public List<Album> Albums { get; set; }
        public List<Podcast> Podcasts { get; set; }
        public IEnumerable<Playlist> Playlists { get; set; }
        public UserDetails UserDetails { get; set; }
        public List<Following> Followings { get; set; }
    }
}
