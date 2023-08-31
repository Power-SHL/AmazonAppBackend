namespace AmazonAppBackend.Exceptions.FriendExceptions;

public class FriendRequestDuplicateException : Exception
{
    public FriendRequestDuplicateException(string message) : base(message) {}
}