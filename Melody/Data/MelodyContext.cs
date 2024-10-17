using Melody.Models;
using Microsoft.EntityFrameworkCore;
namespace Melody.Data
{
    public class MelodyContext : DbContext
    {
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Liked> LikedSongs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<PodcastEpisode> PodcastEpisodes { get; set; }
        public DbSet<ChannelPodcast> ChannelPodcasts { get; set; }
        public DbSet<Following> Following { get; set; }
        public MelodyContext(DbContextOptions<MelodyContext> options) : base(options) { }
    }
}
