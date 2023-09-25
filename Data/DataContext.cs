using AmazonAppBackend.DTO.Friends;
using AmazonAppBackend.DTO.Profiles;
using AmazonAppBackend.DTO.Social;
using Microsoft.EntityFrameworkCore;

namespace AmazonAppBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<UnverifiedProfile> UnverifiedProfiles { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define table names
            modelBuilder.Entity<Profile>().ToTable("Profiles");
            modelBuilder.Entity<FriendRequest>().ToTable("FriendRequests");
            modelBuilder.Entity<Friendship>().ToTable("Friendships");
            modelBuilder.Entity<UnverifiedProfile>().ToTable("UnverifiedProfiles");
            modelBuilder.Entity<ResetPasswordRequest>().ToTable("ResetPasswordRequests");
            modelBuilder.Entity<Post>().ToTable("Posts");

            // Primary and unique keys
            modelBuilder.Entity<Profile>().HasKey(p => p.Username);
            modelBuilder.Entity<Profile>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<FriendRequest>().HasKey(fr => new { fr.Sender, fr.Receiver });
            modelBuilder.Entity<Friendship>().HasKey(fs => new { fs.User1, fs.User2 });

            modelBuilder.Entity<UnverifiedProfile>().HasKey(up => up.Username);
            modelBuilder.Entity<UnverifiedProfile>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<ResetPasswordRequest>().HasKey(rpr => rpr.Username);
            modelBuilder.Entity<ResetPasswordRequest>().HasIndex(rpr => rpr.Code).IsUnique();

            modelBuilder.Entity<Post>().HasKey(p => new {p.Username, p.Platform });

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

            modelBuilder.Entity<ResetPasswordRequest>()
                .HasOne(rpr => rpr.User)
                .WithMany()
                .HasForeignKey(rpr => rpr.Username)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Profile)
                .WithOne()
                .HasForeignKey<Post>(p => p.Username)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}