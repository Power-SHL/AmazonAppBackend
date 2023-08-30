using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Storage.FriendRequestStore;

namespace AmazonAppBackend.Storage.FriendRequestStore;

public class InMemoryRequestStore : IFriendRequestStore
{
    private readonly Dictionary<string, List<FriendRequest>> _friendRequests = new();
    public Task RemoveFriendRequest(FriendRequest request)
    {
        if (_friendRequests.TryGetValue(request.Receiver, out var requests))
        {
            int removedElements = requests.RemoveAll(r => r.Sender == request.Sender);
            if (removedElements > 0)
            {
                return Task.CompletedTask;
            }
        }
        throw new FriendRequestNotFoundException($"No friend request from {request.Sender} to {request.Receiver} found");
    }

    public Task<List<FriendRequest>> GetFriendRequests(string username)
    {
        if (!_friendRequests.ContainsKey(username))
        {
            _friendRequests.Add(username, new List<FriendRequest>());
        }
        var requests = _friendRequests[username];

        if (requests.Count == 0) 
        {
            throw new FriendRequestNotFoundException($"No friend requests found for {username}");
        }
        return Task.FromResult(requests);
        
    }

    public Task SendFriendRequest(FriendRequest request)
    {
        if (!_friendRequests.ContainsKey(request.Receiver))
        {
            _friendRequests.Add(request.Receiver, new List<FriendRequest>());
        }

        var requests = _friendRequests[request.Receiver];
        if (requests.Any(r => r.Sender == request.Sender))
        {
            throw new FriendRequestDuplicateException($"Friend request from {request.Sender} to {request.Receiver} already exists");
        }
        requests.Add(request);
        return Task.CompletedTask;
    }
}
