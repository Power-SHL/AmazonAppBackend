using AmazonAppBackend.DTO.Social;
namespace AmazonAppBackend.Services.PostService;

public interface IFeedService
{
    Task CreateSpotifyPost(Post post);
}