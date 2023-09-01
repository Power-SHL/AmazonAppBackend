using AmazonAppBackend.DTO;
using Microsoft.EntityFrameworkCore;
namespace AmazonAppBackend.Data;

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
        modelBuilder.Entity<FriendRequest>()
            .HasKey(fr => new { fr.Sender, fr.Receiver });

        modelBuilder.Entity<Friendship>()
            .HasKey(f => new { f.User1, f.User2 });

        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Sender)
            .WithMany()
            .HasForeignKey(fr => fr.Sender)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FriendRequest>()
            .HasOne(fr => fr.Receiver)
            .WithMany()
            .HasForeignKey(fr => fr.Receiver)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.User1)
            .WithMany()
            .HasForeignKey(f => f.User1)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friendship>()
            .HasOne(f => f.User2)
            .WithMany()
            .HasForeignKey(f => f.User2)
            .OnDelete(DeleteBehavior.Restrict);
    }
}