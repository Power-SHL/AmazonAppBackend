namespace AmazonAppBackend.Exceptions.FriendExceptions;
public class FriendRequestAcceptedInsteadException : Exception
{
    public FriendRequestAcceptedInsteadException(string message) : base(message) {}
}