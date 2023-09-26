namespace AmazonAppBackend.Exceptions.FeedExceptions;

public class PostDeleteInvalidException : Exception
{
    public PostDeleteInvalidException(string message) : base(message) {}
}