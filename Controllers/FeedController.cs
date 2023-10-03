using AmazonAppBackend.DTO.Feed;
using AmazonAppBackend.DTO.Social;
using AmazonAppBackend.Exceptions.FeedExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
using AmazonAppBackend.Exceptions.FriendExceptions;
using AmazonAppBackend.Extensions;
using AmazonAppBackend.Services.AuthorizationService;
using AmazonAppBackend.Services.FeedService;
using AmazonAppBackend.Services.PostService;
using AmazonAppBackend.Services.ProfileService;
using Microsoft.AspNetCore.Mvc;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/feed")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;
    private readonly IAuthorizationService _authorizationService;
    public FeedController(IFeedService feedService, IAuthorizationService authorizationService)
    {
        _feedService = feedService;
        _authorizationService = authorizationService;
    }

    [HttpPost("spotify")]
    public async Task<ActionResult> AddPost(PostRequest postRequest)
    {
        if (!postRequest.Username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {
            _authorizationService.AuthorizeRequest(User, postRequest.Username);
            Post post = new(postRequest.Username, "spotify", postRequest.ContentId);
            await _feedService.CreateSpotifyPost(post);
            return Ok(post);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile {postRequest.Username} not found");
            }
            if (e is PostDuplicateException)
            {
                return Conflict($"Spotify post by {postRequest.Username} already exists");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePost(DeletePostRequest request)
    {
        try
        {
            request.ValidateDeleteRequest();
            _authorizationService.AuthorizeRequest(User, request.Username);
            await _feedService.DeletePost(request);
            return Ok("Post successfully deleted");
        }
        catch (Exception e)
        {
            if (e is PostDeleteInvalidException)
            {
                return BadRequest(e.Message);
            }
            if (e is PostNotFoundException)
            {
                return NotFound($"{request.Platform} post by {request.Username} not found.");
            }
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetPostsOfFriends(string username, int pageNumber = 1, int pageSize = 12)
    {
        username = username.ToLower();
        if (!username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0");
        }
        try
        {
            _authorizationService.AuthorizeRequest(User, username);
            EnumeratePostResponse response = new(await _feedService.GetPostsOfFriends(username, pageNumber, pageSize), 
                                            username, pageNumber, pageSize);
            return Ok(response);
        }
        catch (Exception e)
        {
            if (e is UnauthorizedAccessException)
            {
                return Unauthorized(e.Message);
            }
            if (e is PostNotFoundException || e is FriendNotFoundException)
            {
                return NotFound($"No more posts found for user {username}");
            }
            throw;
        }
    }
}