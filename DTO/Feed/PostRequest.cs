using System.ComponentModel.DataAnnotations;

namespace AmazonAppBackend.DTO.Feed;

public class PostRequest
{
    public string Username { get; set; }
    public string ContentId { get; set; }

    public PostRequest([Required] string username, [Required] string contentId)
    {
        Username = username.ToLower();
        ContentId = contentId;
    }
}