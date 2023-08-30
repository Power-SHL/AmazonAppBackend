namespace AmazonAppBackend.Exceptions.FriendExceptions;

public class FriendNotFoundException : Exception
{
    public FriendNotFoundException(string message) : base(message) {}
}
