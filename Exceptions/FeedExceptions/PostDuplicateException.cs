namespace AmazonAppBackend.Exceptions.FeedExceptions;

public class PostDuplicateException : Exception
{
    public PostDuplicateException(string message) : base(message) { }
}
