using AmazonAppBackend.Services.PostService;
using Microsoft.AspNetCore.Mvc;

namespace AmazonAppBackend.Controllers;

[ApiController]
[Route("api/feed")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;
    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }
}
