using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services.FriendService;
public interface IFriendService
{
    Task SendFriendRequest(FriendRequest request);
    Task AcceptFriendRequest(FriendRequest request);
    Task RemoveFriend(FriendRequest request);
    Task RemoveFriendRequest(FriendRequest request);
    Task<List<FriendRequest>> GetSentFriendRequests(string username);
    Task<List<FriendRequest>> GetReceivedFriendRequests(string username);
    Task<List<Friend>> GetFriends(string username);
}