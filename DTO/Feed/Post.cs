using AmazonAppBackend.DTO.Profiles;
using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO.Social;

public record Post
{
    public string Username { get; set; }
    public string Platform { get; set; }
    public string ContentId { get; set; }
    public long TimeCreated { get; set; }

    public Post(string username, string platform, string contentId)
    {
        Username = username.ToLower();
        Platform = platform;
        ContentId = contentId;
        TimeCreated = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public Profile Profile;
}