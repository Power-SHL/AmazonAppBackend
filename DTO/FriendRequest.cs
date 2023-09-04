using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO;

public record FriendRequest
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public long TimeAdded { get; }

    public Profile SenderProfile;
    public Profile ReceiverProfile;

    public FriendRequest([Required] string sender, [Required] string receiver)
    {
        Sender = sender;
        Receiver = receiver;
        TimeAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}