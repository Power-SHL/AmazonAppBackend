namespace AmazonAppBackend.Exceptions.FriendExceptions;

public class FriendRequestInvalidException : Exception
{
    public FriendRequestInvalidException(string message) : base(message) { }
}
