namespace AmazonAppBackend.Exceptions.FeedExceptions;
public class PostNotFoundException : Exception
{
    public PostNotFoundException(string message) : base(message) {}
}