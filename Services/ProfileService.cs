using AmazonAppBackend.DTO;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ProfileStore;

namespace AmazonAppBackend.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileStore _profileStore;
    private readonly IFriendRequestStore _friendStore;

    public ProfileService(IProfileStore profileStore, IFriendRequestStore friendStore)
    {
        _profileStore = profileStore;
        _friendStore = friendStore;
    }
    public async Task<Profile> CreateProfile(Profile profile)
    {
        await _profileStore.CheckUniqueEmail(profile.Email);
        return await _profileStore.CreateProfile(profile);
    }

    public async Task DeleteProfile(string username)
    {

        await _profileStore.DeleteProfile(username);
    }

    public async Task<Profile> GetProfile(string username)
    {
        return await _profileStore.GetProfile(username);
    }

    public async Task<Profile> UpdateProfile(Profile profile)
    {
        return await _profileStore.UpdateProfile(profile);
    }
}