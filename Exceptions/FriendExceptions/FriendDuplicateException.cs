namespace AmazonAppBackend.Exceptions.FriendExceptions;

public class FriendDuplicateException : Exception
{
    public FriendDuplicateException(string message) : base(message) {}
}
