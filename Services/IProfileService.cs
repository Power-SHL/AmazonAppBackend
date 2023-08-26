using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services;
public interface IProfileService
{
    Task<Profile> GetProfile(string username);
    Task<Profile> GetProfileByEmail(string email);
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(string username, Profile profile);
    Task DeleteProfile(string username);
}