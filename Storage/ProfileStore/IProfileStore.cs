using AmazonAppBackend.DTO;
namespace AmazonAppBackend.Storage.ProfileStore;
public interface IProfileStore
{
    Task<Profile> GetProfile(string username);
    Task<Profile> GetProfileByEmail(string email);
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task AddFriend (string friend1, string friend2);
    Task RemoveFriend(string friend1, string friend2);
    Task DeleteProfile(string username);
    Task<List<Friend>> GetFriends (string username);
}