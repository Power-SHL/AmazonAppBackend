using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Storage.FriendRequestStore;

public interface IFriendRequestStore
{
    Task SendFriendRequest(FriendRequest request);
    Task RemoveFriendRequest(FriendRequest request);
    Task<List<FriendRequest>> GetSentFriendRequests(string username);
    Task<List<FriendRequest>> GetReceivedFriendRequests(string username);
}