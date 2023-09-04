﻿using AmazonAppBackend.Data;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Storage.ProfileStore;
using AmazonAppBackend.Storage.FriendRequestStore;
using Microsoft.EntityFrameworkCore;

namespace AmazonAppBackend.Storage;

public class PostgreSqlStore : IProfileStore, IFriendRequestStore
{
    private readonly DataContext _context;

    public PostgreSqlStore(DataContext context)
    {
        _context = context;
    }

    public async Task<Profile> GetProfile(string username)
    {
        var profile = await _context.Profiles.FindAsync(username);
        return profile ?? throw new ProfileNotFoundException($"Profile {username} not found");
    }

    public async Task<Profile> GetProfileByEmail(string email)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Email == email);
        return profile ?? throw new ProfileNotFoundException($"Profile with email {email} not found");
    }

    public async Task<Profile> CreateProfile(Profile profile)
    {
        try
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return profile;
        }
        catch (DbUpdateException)
        {
            throw new ProfileAlreadyExistsException($"Profile {profile.Username} already exists");
        }
    }
    public async Task<Profile> UpdateProfile(Profile profile)
    {
        _context.Profiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task DeleteProfile(string username)
    {
        var profile = await _context.Profiles.FindAsync(username) 
                      ?? throw new ProfileNotFoundException($"Profile {username} not found.");
        _context.Profiles.Remove(profile);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Friend>> GetFriends(string username)
    {
        var friends = await _context.Friendships
            .Where(friendship => friendship.User1 == username || friendship.User2 == username)
            .Select(friendship => new Friend(friendship.User1 == username ? friendship.User2 : friendship.User1, friendship.TimeAdded))
            .ToListAsync();

        return friends ?? throw new FriendRequestNotFoundException();
    }

    public async Task AddFriend(string friend1, string friend2)
    {
        Friendship friendship = string.Compare(friend1, friend2, StringComparison.OrdinalIgnoreCase) < 0 ? 
                                new Friendship(friend1, friend2) : new Friendship(friend2, friend1);
        try
        {
            await _context.Friendships.AddAsync(friendship);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new FriendDuplicateException($"Friendship between {friend1} and {friend2} already exists");
        }
    }

    public async Task RemoveFriend(string friend1, string friend2)
    {
        Friendship friendship = string.Compare(friend1, friend2, StringComparison.OrdinalIgnoreCase) < 0 ?
            new Friendship(friend1, friend2) : new Friendship(friend2, friend1);

        _context.Friendships.Remove(friendship);
        if (await _context.SaveChangesAsync() == 0)
        {
            throw new FriendNotFoundException($"Friendship between {friend1} and {friend2} not found");
        }
    }

    public async Task SendFriendRequest(FriendRequest request)
    {
        try
        {
            var friendRequest = await _context.Friend_Requests
                .FirstOrDefaultAsync(r => r.Sender == request.Receiver && r.Receiver == request.Sender);
            if (friendRequest != null)
            {
                Friendship friendship = string.Compare(request.Sender, request.Receiver, StringComparison.OrdinalIgnoreCase) < 0 ?
                    new Friendship(request.Sender, request.Receiver) : new Friendship(request.Receiver, request.Sender);

                await _context.Friendships.AddAsync(friendship);
                _context.Friend_Requests.Remove(friendRequest);
                await _context.SaveChangesAsync();
                throw new FriendRequestAcceptedInsteadException($"{request.Sender} and {request.Receiver} are now friends");

            }
            else
            {
                _context.Friend_Requests.Add(request);
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException)
        {
            throw new FriendRequestDuplicateException($"Friend request from {request.Sender} to {request.Receiver} already exists.");
        }
    }

    public async Task RemoveFriendRequest(FriendRequest request)
    {
        _context.Friend_Requests.Remove(request);
        if (await _context.SaveChangesAsync() == 0)
        {
            throw new FriendRequestNotFoundException($"Friend request from {request.Sender} to {request.Receiver} not found");
        }
    }

    public async Task<List<FriendRequest>> GetReceivedFriendRequests(string username)
    {
        var friendRequests = await _context.Friend_Requests
            .Where(request => request.Receiver == username)
            .ToListAsync();

        return friendRequests ?? throw new FriendRequestNotFoundException($"No friend requests sent to {username}");
    }

    public async Task<List<FriendRequest>> GetSentFriendRequests(string username)
    {
        var friendRequests = await _context.Friend_Requests
            .Where(request => request.Sender == username)
            .ToListAsync();

        return friendRequests ?? throw new FriendRequestNotFoundException($"No friend requests sent by {username}");
    }
}