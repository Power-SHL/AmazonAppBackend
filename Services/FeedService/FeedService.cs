using AmazonAppBackend.DTO.Social;
using AmazonAppBackend.Services.PostService;
using AmazonAppBackend.Services.ProfileService;
using AmazonAppBackend.Storage.FeedStore;

namespace AmazonAppBackend.Services.FeedService;

public class FeedService : IFeedService
{
    private readonly IProfileService _profileService;
    private readonly ISpotifyService _spotifyService;
    private readonly IFeedStore _feedStore;

    public FeedService(IProfileService profileService, ISpotifyService spotifyService, IFeedStore feedStore)
    {
        _profileService = profileService;
        _spotifyService = spotifyService;
        _feedStore = feedStore;
    }
    public async Task CreateSpotifyPost(Post post)
    {
        string token = await _spotifyService.GetToken();
        await Task.WhenAll(_profileService.GetProfile(post.Username), _spotifyService.GetSong(post.ContentId, token));
        await _feedStore.CreatePost(post);
    }
}