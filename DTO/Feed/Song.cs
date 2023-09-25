namespace AmazonAppBackend.DTO.Social;

public record Song
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public List<string> Artists { get; set; }

    public Song(string id, string name, string imageUrl, List<string> artists)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        Artists = artists;
    }
}
