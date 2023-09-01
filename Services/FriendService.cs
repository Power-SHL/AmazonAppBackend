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

    public async Task<List<FriendRequest>> GetSentFriendRequests(string username)
    {
        return await _friendStore.GetSentFriendRequests(username);
    }

    public async Task<List<FriendRequest>> GetReceivedFriendRequests(string username)
    {
        return await _friendStore.GetReceivedFriendRequests(username);
    }

    public async Task SendFriendRequest(FriendRequest request)
    {
        await _profileStore.CheckProfilesExist(new List<string> { request.Sender, request.Receiver });
        try
        {
            var friendRequests = await GetReceivedFriendRequests(request.Sender);
            if (friendRequests.Any(r => r.Sender == request.Receiver)) // if sender has received a request from receiver, accept it
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
        await _profileStore.AddFriend(request.Sender, request.Receiver);
    }

    public async Task<List<Friend>> GetFriends(string username)
    {
        return await _profileStore.GetFriends(username);
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