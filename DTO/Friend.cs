using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO;

public record Friend
{
    public string Username { get; set; }
    public long TimeAdded;

    public Friend([Required] string username)
    {
        Username = username;
        TimeAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}