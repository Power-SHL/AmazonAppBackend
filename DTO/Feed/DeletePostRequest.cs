using System.ComponentModel.DataAnnotations;
namespace AmazonAppBackend.DTO.Feed;

public record DeletePostRequest
{
    public string Username { get; init; }
    public string Platform { get; init; }

    public DeletePostRequest([Required] string username, [Required] string platform)
    {
        Username = username.ToLower();
        Platform = platform.ToLower();
    }
}