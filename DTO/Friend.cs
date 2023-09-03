namespace AmazonAppBackend.DTO;
public record Friend
{
    public string Username { get; set; }
    public long TimeAdded { get; set; }

    public Friend(string username, long timeAdded)
    {
        Username = username;
        TimeAdded = timeAdded;
    }
}