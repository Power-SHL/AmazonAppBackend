using System.ComponentModel.DataAnnotations;
using AmazonAppBackend.DTO.Profiles;

namespace AmazonAppBackend.DTO.Friends;
public record Friendship
{
    public string User1 { get; set; }
    public string User2 { get; set; }
    public long TimeAdded { get; set; }
    public Profile User1Profile;
    public Profile User2Profile;

    public Friendship([Required] string user1, [Required] string user2)
    {
        User1 = user1.ToLower();
        User2 = user2.ToLower();
        TimeAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}