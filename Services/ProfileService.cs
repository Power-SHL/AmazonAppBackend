using AmazonAppBackend.DTO;
namespace AmazonAppBackend.Services;

public class ProfileService : IProfileService
{
    public Task<Profile> CreateProfile(Profile profile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProfile(string username)
    {
        throw new NotImplementedException();
    }

    public Task<Profile> GetProfile(string username)
    {
        throw new NotImplementedException();
    }

    public Task<Profile> GetProfileByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Profile> UpdateProfile(string username, Profile profile)
    {
        throw new NotImplementedException();
    }
}