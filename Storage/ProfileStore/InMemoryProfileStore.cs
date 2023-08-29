using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AmazonAppBackend.Storage.ProfileStore;

public class InMemoryProfileStore : IProfileStore
{
    private readonly Dictionary<string, Profile> _profiles = new();

    public Task<Profile> GetProfile(string username)
    {
        if (_profiles.TryGetValue(username, out var profile))
        {
            return Task.FromResult(profile);
        }

        throw new ProfileNotFoundException($"Profile with username {username} not found.");
    }

    public Task<Profile> GetProfileByEmail(string email)
    {
        var profile = _profiles.Values.FirstOrDefault(p => p.Email == email);
        if (profile != null)
        {
            return Task.FromResult(profile);
        }
        throw new ProfileNotFoundException();
    }

    public Task<Profile> CreateProfile(Profile profile)
    {
        try
        {
            _profiles.Add(profile.Username, profile);
            return Task.FromResult(profile);
        }
        catch (ArgumentException)
        {
            throw new ProfileAlreadyExistsException();
        }

    }

    public Task<Profile> UpdateProfile(Profile profile)
    {
        if (_profiles.Remove(profile.Username))
        {
            _profiles[profile.Username] = profile;
            return Task.FromResult(profile);
        }
        else
        {
            throw new ProfileNotFoundException($"Profile with username {profile.Username} not found.");
        }
    }

    public Task DeleteProfile(string username)
    {
        return _profiles.Remove(username) ? Task.CompletedTask : throw new ProfileNotFoundException();
    }
}   