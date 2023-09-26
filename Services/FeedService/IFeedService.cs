using AmazonAppBackend.DTO.Feed;
using AmazonAppBackend.DTO.Social;
namespace AmazonAppBackend.Services.PostService;

public interface IFeedService
{
    Task CreateSpotifyPost(Post post);
    Task DeletePost(DeletePostRequest request);
    Task<List<Post>> GetPostsOfFriends(string username, int pageNumber, int pageSize);
}