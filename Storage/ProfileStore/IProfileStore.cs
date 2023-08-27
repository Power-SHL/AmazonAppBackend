using AmazonAppBackend.DTO;
namespace AmazonAppBackend.Storage.ProfileStore;
public interface IProfileStore
{
    Task<Profile> GetProfile(string username);
    Task<Profile> GetProfileByEmail(string email);
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task DeleteProfile(string username);
}