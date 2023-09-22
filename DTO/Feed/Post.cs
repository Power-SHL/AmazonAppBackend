namespace AmazonAppBackend.DTO.Social;

public record Post
{
    public string Username { get; set; }
    public string Platform { get; set; }
    public string ContentId { get; set; }
}