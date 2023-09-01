namespace AmazonAppBackend.DTO;
public record Friend
{
    public string Username;
    public long TimeAdded;

    public Friend(string username, long timeAdded)
    {
        Username = username;
        TimeAdded = timeAdded;
    }
}