using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Storage.ProfileStore;

namespace AmazonAppBackend.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileStore _profileStore;

    public ProfileService(IProfileStore profileStore)
    {
        _profileStore = profileStore;
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