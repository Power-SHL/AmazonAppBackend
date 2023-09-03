using AmazonAppBackend.DTO;
using Microsoft.EntityFrameworkCore;

namespace AmazonAppBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<FriendRequest> Friend_Requests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Profile>()
                .HasKey(p => p.Username);

            modelBuilder.Entity<FriendRequest>()
                .HasKey(fr => new { fr.Sender, fr.Receiver });

            modelBuilder.Entity<Friendship>()
                .HasKey(f => new { f.User1, f.User2 });

        }
    }
}