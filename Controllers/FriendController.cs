﻿using Microsoft.AspNetCore.Mvc;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Services.FriendService;
using AmazonAppBackend.Services.AuthorizationService;
using AmazonAppBackend.DTO.Friends;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/friends")]
public class FriendController : ControllerBase
{
    private readonly IFriendService _friendService;
    private readonly IAuthorizationService _authorizationService;
    public FriendController(IFriendService friendService, IAuthorizationService authorizationService)
    {
        _friendService = friendService;
        _authorizationService = authorizationService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<List<Friend>>> GetFriends(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            var friends = await _friendService.GetFriends(username);
            return Ok(friends);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile with username {username} not found.");
            }
            if (e is FriendNotFoundException)
            {
                return NotFound($"No friends found for user {username}");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }

            throw;
        }
    }
    
    [HttpGet("requests/{username}/received")]
    public async Task<ActionResult<List<Friend>>> GetReceivedFriendRequests(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            var friendRequests = await _friendService.GetReceivedFriendRequests(username);
            return Ok(friendRequests);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile with username {username} not found.");
            }
            else if (e is FriendRequestNotFoundException)
            {
                return NotFound($"No friend requests found for user {username}");
            }
            else if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }

    [HttpGet("requests/{username}/sent")]
    public async Task<ActionResult<List<Friend>>> GetSentFriendRequests(string username)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            var friendRequests = await _friendService.GetSentFriendRequests(username);
            return Ok(friendRequests);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile with username {username} not found.");
            }
            if (e is FriendRequestNotFoundException)
            {
                return NotFound($"No friend requests found for user {username}");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }

            throw;
        }
    }

    [HttpPost("send")]
    public async Task<ActionResult> SendFriendRequest(CreateFriendRequest createRequest)
    {
        FriendRequest request = new(createRequest);
        try
        {
            _authorizationService.AuthorizeRequest(User, createRequest.Sender);
            request.ValidateFriendRequest();
            await _friendService.SendFriendRequest(request);
            return Created($"api/friends/{request.Sender}",
                $"{request.Sender} sent a friend request to {request.Receiver}");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound(e.Message);
            }
            else if (e is FriendRequestInvalidException)
            {
                return BadRequest(e.Message);
            }
            else if (e is FriendRequestDuplicateException)
            {
                return Conflict($"{request.Sender} has already sent a friend request to {request.Receiver}");
            }
            else if (e is FriendDuplicateException)
            {
                return BadRequest($"{request.Sender} and {request.Receiver} are already friends");
            }
            else if (e is FriendRequestAcceptedInsteadException)
            {
                return Created($"api/friends/{request.Sender}",
                    $"{request.Sender} and {request.Receiver} are now friends");
            }
            else if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }

            throw;
        }
    }

    [HttpPost("accept")]
    public async Task<ActionResult> AcceptFriendRequest(FriendRequest request)
    {
        try
        {
            _authorizationService.AuthorizeRequest(User, request.Receiver);
            request.ValidateFriendRequest();
            await _friendService.AcceptFriendRequest(request);
            return Created($"api/friends/{request.Sender}", $"{request.Sender} and {request.Receiver} are now friends");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound(e.Message);
            }
            else if (e is FriendRequestInvalidException)
            {
                return BadRequest(e.Message);
            }
            else if (e is FriendRequestNotFoundException)
            {
                return NotFound($"No friend request from {request.Sender} to {request.Receiver} was found");
            }
            else if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }

            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveFriend(FriendRequest request)
    {
        try
        {
            _authorizationService.AuthorizeRequest(User, new List<string>() {request.Sender, request.Receiver });
            request.ValidateFriendRequest();
            await _friendService.RemoveFriend(request);
            return Ok($"{request.Sender} and {request.Receiver} are no longer friends");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound(e.Message);
            }
            else if (e is FriendRequestInvalidException)
            {
                return BadRequest(e.Message);
            }

            if (e is FriendNotFoundException)
            {
                return NotFound($"No friend relationship between {request.Sender} and {request.Receiver} was found.");
            }
            if(e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }

    [HttpDelete("request")]
    public async Task<ActionResult> RemoveFriendRequest(FriendRequest request)
    {
        try
        {
            _authorizationService.AuthorizeRequest(User, new List<string>() { request.Sender, request.Receiver });
            request.ValidateFriendRequest();
            await _friendService.RemoveFriendRequest(request);
            return Ok($"Friend request from {request.Sender} to {request.Receiver} was removed");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound(e.Message);
            }
            else if (e is FriendRequestInvalidException)
            {
                return BadRequest(e.Message);
            }
            else if (e is FriendRequestNotFoundException)
            {
                return NotFound($"No friend request from {request.Sender} to {request.Receiver} was found");
            }
            else if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }
}