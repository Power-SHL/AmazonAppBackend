namespace AmazonAppBackend.Exceptions.ImageExceptions;

public class ImageNotFoundException : Exception
{
    public ImageNotFoundException(string message) : base(message) { }
}