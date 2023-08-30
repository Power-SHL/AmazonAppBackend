using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using System.Collections.Generic;

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

    public Task AddFriend(string friend1, string friend2)
    {
        if (_profiles.TryGetValue(friend1, out var profile))
        {
            profile.Friends.Add(new Friend(friend2));
            return Task.CompletedTask;
        }
        throw new ProfileNotFoundException($"{friend1} was not found.");
    }

    public Task RemoveFriend(string friend1, string friend2)
    {
        if (_profiles.TryGetValue(friend1, out var profile))
        {
            int removedElements = profile.Friends.RemoveAll(user => user.Username == friend2);
            if(removedElements == 0)
            {
                throw new FriendNotFoundException($"{friend2} is not listed as a friend of {friend1}.");
            }
            return Task.CompletedTask;
        }
        throw new ProfileNotFoundException($"{friend1} was not found.");
    }
}