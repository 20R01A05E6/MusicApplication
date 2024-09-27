using Microsoft.EntityFrameworkCore;

namespace Melody.Data
{
    public class MelodyContext : DbContext
    {
        public MelodyContext (DbContextOptions<MelodyContext> options)
            : base(options)
        {
        }

        public DbSet<Melody.Models.UserDetails> UserDetails { get; set; } = default!;
        public DbSet<Melody.Models.Playlists> Playlists { get; set; } = default!;
        public DbSet<Melody.Models.Song> Song { get; set; } = default!;
        public DbSet<Melody.Models.Liked> Liked { get; set; } = default!;
    }
}
