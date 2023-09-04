using AmazonAppBackend.DTO;
using Microsoft.EntityFrameworkCore;

namespace AmazonAppBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<FriendRequest> Friend_Requests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define table names
            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<FriendRequest>().ToTable("friend_requests");
            modelBuilder.Entity<Friendship>().ToTable("Friendship");

            // Primary keys
            modelBuilder.Entity<Profile>().HasKey(p => p.Username);
            modelBuilder.Entity<FriendRequest>().HasKey(fr => new { fr.Sender, fr.Receiver });
            modelBuilder.Entity<Friendship>().HasKey(fs => new { fs.User1, fs.User2 });

            // Define foreign key relationships
            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.SenderProfile)
                .WithMany()
                .HasForeignKey(fr => fr.Sender)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(fr => fr.ReceiverProfile)
                .WithMany()
                .HasForeignKey(fr => fr.Receiver)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.User1Profile)
                .WithMany()
                .HasForeignKey(fs => fs.User1)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Friendship>()
                .HasOne(fs => fs.User2Profile)
                .WithMany()
                .HasForeignKey(fs => fs.User2)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}