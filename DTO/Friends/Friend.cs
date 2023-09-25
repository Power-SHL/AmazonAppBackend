namespace AmazonAppBackend.DTO.Friends;
public record Friend
{
    public string Username { get; set; }
    public long TimeAdded { get; set; }

    public Friend(string username, long timeAdded)
    {
        Username = username.ToLower();
        TimeAdded = timeAdded;
    }
}