namespace AmazonAppBackend.Exceptions.FriendExceptions;

public class FriendRequestNotFoundException : Exception
{
    public FriendRequestNotFoundException(string message) : base(message) {}
}
