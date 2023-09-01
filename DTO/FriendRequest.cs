using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record FriendRequest
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public long TimeSent;

    public FriendRequest([Required] string sender, [Required] string receiver)
    {
        Sender = sender;
        Receiver = receiver;
        TimeSent = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}