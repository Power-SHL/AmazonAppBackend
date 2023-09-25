using AmazonAppBackend.DTO.Profiles;
using AmazonAppBackend.Exceptions.ImageExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services.ImageService;
using AmazonAppBackend.Storage.ProfileStore;

namespace AmazonAppBackend.Services.ProfileService;

public class ProfileService : IProfileService
{
    private readonly IProfileStore _profileStore;
    private readonly IImageService _imageService;

    public ProfileService(IProfileStore profileStore, IImageService imageService)
    {
        _profileStore = profileStore;
        _imageService = imageService;
    }
    public async Task<UnverifiedProfile> CreateProfile(UnverifiedProfile profile)
    {
        await _profileStore.CheckUniqueEmail(profile.Email);
        return await _profileStore.CreateProfile(profile);
    }

    public async Task DeleteProfile(string username)
    {
        try
        {
            await Task.WhenAll(_profileStore.DeleteProfile(username), _imageService.DeleteImage(username));
        }
        catch (ImageNotFoundException) { }
    }

    public async Task<Profile> GetProfile(string username)
    {
        return await _profileStore.GetProfile(username);
    }

    public async Task<Profile> GetProfileByEmail(string email)
    {
        return await _profileStore.GetProfileByEmail(email);
    }

    public async Task<Profile> VerifyProfile(string username, string verificationCode)
    {
        var profile = await _profileStore.GetUnverifiedProfile(username);
        if (profile.VerificationCode == verificationCode)
        {
            return await _profileStore.VerifyProfile(username);
        }
        throw new ProfileVerificationException("Verification code is incorrect");
    }

    public async Task<UnverifiedProfile> GetUnverifiedProfile(string username)
    {
        return await _profileStore.GetUnverifiedProfile(username);
    }

    public async Task<Profile> UpdateProfile(Profile profile)
    {
        try
        {
            var existingProfile = await _profileStore.GetProfileByEmail(profile.Email);
            if (existingProfile.Username != profile.Username)
            {
                throw new ProfileDuplicateException($"Profile {profile.Email} already in use.");
            }

            return await _profileStore.UpdateProfile(profile);
            
        }
        catch (ProfileNotFoundException)
        {
            return await _profileStore.UpdateProfile(profile);
        }
    }

    public async Task AddResetPasswordRequest(ResetPasswordRequest request)
    {
        await _profileStore.CheckProfilesExist(new List<string>(){request.Username});
        await _profileStore.AddResetPasswordRequest(request);
    }

    public async Task<ResetPasswordRequest> GetResetPasswordRequest(string username)
    {
        return await _profileStore.GetResetPasswordRequest(username);
    }

    public async Task ResetPassword(ChangedPasswordRequest request)
    {
        await _profileStore.ResetPassword(request);
    }
}