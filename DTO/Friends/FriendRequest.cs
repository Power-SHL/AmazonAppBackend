using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AmazonAppBackend.DTO.Profiles;

namespace AmazonAppBackend.DTO.Friends;

public record FriendRequest
{
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public long TimeAdded { get; set; }

    public Profile SenderProfile;
    public Profile ReceiverProfile;

    [JsonConstructor]
    public FriendRequest(string sender, string receiver)
    {
        Sender = sender.ToLower();
        Receiver = receiver.ToLower();
        TimeAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public FriendRequest(CreateFriendRequest request)
    {
        Sender = request.Sender.ToLower();
        Receiver = request.Receiver.ToLower();
        TimeAdded = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}