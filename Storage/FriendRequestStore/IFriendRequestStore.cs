using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Storage.FriendRequestStore;

public interface IFriendRequestStore
{
    Task SendFriendRequest(FriendRequest request);
    Task RemoveFriendRequest(FriendRequest request);
    Task<List<FriendRequest>> GetFriendRequests(string username);
}