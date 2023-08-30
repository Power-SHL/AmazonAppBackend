using AmazonAppBackend.DTO;
namespace AmazonAppBackend.Services;
public interface IFriendService
{
    Task SendFriendRequest(FriendRequest request);
    Task AcceptFriendRequest(FriendRequest request);
    Task RemoveFriend(FriendRequest request);
    Task RemoveFriendRequest(FriendRequest request);
    Task<List<FriendRequest>> GetFriendRequests(string username);
    Task<List<Friend>> GetFriends(string username);
    Task DeleteFriendsAndRequests(string username);
}