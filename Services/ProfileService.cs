using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ImageExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Storage.FriendRequestStore;
using AmazonAppBackend.Storage.ImageStore;
using AmazonAppBackend.Storage.ProfileStore;

namespace AmazonAppBackend.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileStore _profileStore;
    private readonly IImageService _imageService;

    public ProfileService(IProfileStore profileStore, IImageService imageService)
    {
        _profileStore = profileStore;
        _imageService = imageService;
    }
    public async Task<Profile> CreateProfile(Profile profile)
    {
        await _profileStore.CheckUniqueEmail(profile.Email);
        return await _profileStore.CreateProfile(profile);
    }

    public async Task DeleteProfile(string username)
    {
        await Task.WhenAll(_profileStore.DeleteProfile(username), _imageService.DeleteImage(username));
    }

    public async Task<Profile> GetProfile(string username)
    {
        return await _profileStore.GetProfile(username);
    }

    public async Task<Profile> UpdateProfile(Profile profile)
    {
        try
        {
            var existingProfile = await _profileStore.GetProfileByEmail(profile.Email);
            if (existingProfile.Username != profile.Username)
            {
                throw new ProfileAlreadyExistsException($"Profile {profile.Email} already in use.");
            }
            else
            {
                return await _profileStore.UpdateProfile(profile);
            }
        }
        catch (ProfileNotFoundException)
        {
            return await _profileStore.UpdateProfile(profile);
        }
    }
}