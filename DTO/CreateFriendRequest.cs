using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO;

public record CreateFriendRequest
{
    public string Sender { get; set; }
    public string Receiver { get; set; }

    public CreateFriendRequest([Required] string sender, [Required] string receiver)
    {
        Sender = sender.ToLower();
        Receiver = receiver.ToLower();
    }
}
