using AmazonAppBackend.DTO.Feed;
using AmazonAppBackend.DTO.Social;
namespace AmazonAppBackend.Storage.FeedStore;

public interface IFeedStore
{
    Task CreatePost(Post post);
    Task DeletePost(DeletePostRequest request);
    Task<List<Post>> GetPostsOfFriends(string username, int pageNumber, int pageSize);
}