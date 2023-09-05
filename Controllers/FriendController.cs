using Microsoft.AspNetCore.Mvc;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services;
using AmazonAppBackend.Exceptions.FriendExceptions;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/friends")]
public class FriendController : ControllerBase
{
    private readonly IFriendService _friendService;
    public FriendController(IFriendService friendService)
    {
        _friendService = friendService;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<List<Friend>>> GetFriends(string username)
    {
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            var friends = await _friendService.GetFriends(username);
            return Ok(friends);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile with username {username} not found.");
            }
            else if (e is FriendNotFoundException)
            {
                return NotFound($"No friends found for user {username}");
            }

            throw;
        }
    }
    
    [HttpGet("requests/{username}/received")]
    public async Task<ActionResult<List<Friend>>> GetReceivedFriendRequests(string username)
    {
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
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

            throw;
        }
    }

    [HttpGet("requests/{username}/sent")]
    public async Task<ActionResult<List<Friend>>> GetSentFriendRequests(string username)
    {
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }

        try
        {
            var friendRequests = await _friendService.GetSentFriendRequests(username);
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

            throw;
        }
    }

    [HttpPost("send")]
    public async Task<ActionResult> SendFriendRequest(CreateFriendRequest createRequest)
    {
        FriendRequest request = new(createRequest);
        try
        {
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

            throw;
        }
    }

    [HttpPost("accept")]
    public async Task<ActionResult> AcceptFriendRequest(FriendRequest request)
    {
        try
        {
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

            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveFriend(FriendRequest request)
    {
        try
        {
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

            throw;
        }
    }

    [HttpDelete("request")]
    public async Task<ActionResult> RemoveFriendRequest(FriendRequest request)
    {
        try
        {
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
            throw;
        }
    }
}