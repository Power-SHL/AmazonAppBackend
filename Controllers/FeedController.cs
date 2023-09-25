using AmazonAppBackend.DTO.Feed;
using AmazonAppBackend.DTO.Social;
using AmazonAppBackend.Exceptions.FeedExceptions;
using AmazonAppBackend.Exceptions.ProfileExceptions;
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
        if(!postRequest.Username.IsValidUsername())
        {
            return BadRequest("Username format is invalid");
        }
        try
        {   
            _authorizationService.AuthorizeRequest(User, postRequest.Username);
            Post post = new(postRequest.Username, "Spotify", postRequest.ContentId);
            await _feedService.CreateSpotifyPost(post);
            return Ok(post);

        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Profile {postRequest.Username} not found");
            }
            if(e is PostDuplicateException)
            {
                return Conflict($"Spotify post by {postRequest.Username} already exists");
            }
            throw;
        }
    }
}
