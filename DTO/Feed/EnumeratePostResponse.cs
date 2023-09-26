using AmazonAppBackend.DTO.Social;

namespace AmazonAppBackend.DTO.Feed;

public record EnumeratePostResponse
{
    public List<Post> Posts { get; set; }
    public string NextURI { get; set; }

    public EnumeratePostResponse(List<Post> posts, string username, int pageNumber, int pageSize)
    {
        Posts = posts;
        if(posts.Count == pageSize )
        {
              NextURI = $"/api/feed?username={username}&pageNumber={pageNumber+1}&pageSize={pageSize}";
        }
        else
        {
            NextURI = "";
        }
    }
}