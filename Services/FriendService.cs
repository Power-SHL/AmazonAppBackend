using AmazonAppBackend.Storage.ProfileStore;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Storage.FriendRequestStore;

namespace AmazonAppBackend.Services;

public class FriendService : IFriendService
{
    private readonly IProfileStore _profileStore;
    private readonly IFriendRequestStore _friendStore;

    public FriendService(IProfileStore profileStore, IFriendRequestStore friendStore)
    {
        _profileStore = profileStore;
        _friendStore = friendStore;
    }

    public async Task<List<FriendRequest>> GetFriendRequests(string username)
    {
        return await _friendStore.GetFriendRequests(username);
    }

    public async Task SendFriendRequest(FriendRequest request)
    {
        await _profileStore.CheckProfilesExist(new List<string> { request.Sender, request.Receiver });
        try
        {
            var friendRequests = await GetFriendRequests(request.Sender);
            if (friendRequests.Any(r => r.Sender == request.Receiver)) // if receiver has sent a friend request to sender, accept it
            {
                await AcceptFriendRequest(new FriendRequest(request.Receiver, request.Sender));
                throw new FriendRequestAcceptedInsteadException($"{request.Sender} accepted the request of {request.Receiver}");
            }
            else
            {
                await CheckIfAlreadyFriends(request.Sender, request.Receiver);
                await _friendStore.SendFriendRequest(request);
            }
        }
        catch (FriendRequestNotFoundException)
        {
            await CheckIfAlreadyFriends(request.Sender, request.Receiver);
            await _friendStore.SendFriendRequest(request);
        }
    }

    public async Task AcceptFriendRequest(FriendRequest request)
    {
        await _friendStore.RemoveFriendRequest(request);
        await Task.WhenAll(_profileStore.AddFriend(request.Sender, request.Receiver),
                            _profileStore.AddFriend(request.Receiver, request.Sender)
                            );
    }

    public async Task<List<Friend>> GetFriends(string username)
    {
        var friends = (await _profileStore.GetProfile(username)).Friends;
        if (friends.Count == 0)
        {
            throw new FriendNotFoundException($"No friends found for {username}");
        }
        return friends;
    }

    public async Task RemoveFriend(FriendRequest request)
    {
        await Task.WhenAll(_profileStore.RemoveFriend(request.Sender, request.Receiver),
                            _profileStore.RemoveFriend(request.Receiver, request.Sender)
                            );
    }

    public async Task RemoveFriendRequest(FriendRequest request)
    {
        await _friendStore.RemoveFriendRequest(request);
    }
    public async Task DeleteFriendsAndRequests(string username)
    {
        await Task.WhenAll(RemoveAllFriends(username), RemoveAllRequests(username));
    }

    private async Task RemoveAllFriends(string username)
    {
        try
        {
            var friends = await GetFriends(username);
            foreach (var friend in friends)
            {
                await RemoveFriend(new FriendRequest(username, friend.Username));
            }
        }
        catch (FriendNotFoundException)
        {
            return;
        }
    }

    private async Task RemoveAllRequests(string username)
    {
        try
        {
            var friendRequests = await GetFriendRequests(username);
            foreach (var request in friendRequests)
            {
                await RemoveFriendRequest(new FriendRequest(username, request.Receiver));
            }
        }
        catch (FriendRequestNotFoundException)
        {
            return;
        }
    }

    private async Task CheckIfAlreadyFriends(string sender, string receiver)
    {
        try
        {
            var friends = await GetFriends(sender);
            if (friends.Any(f => f.Username == receiver))
            {
                throw new FriendDuplicateException($"{sender} and {receiver} are already friends.");
            }
        }
        catch (FriendNotFoundException)
        {
            return;
        }
    }
}