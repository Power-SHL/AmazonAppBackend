using AmazonAppBackend.DTO.Social;
namespace AmazonAppBackend.Storage.FeedStore;

public interface IFeedStore
{
    Task CreatePost(Post post);
}