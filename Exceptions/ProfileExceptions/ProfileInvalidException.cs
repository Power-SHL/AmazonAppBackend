namespace AmazonAppBackend.Exceptions.ProfileExceptions;

public class ProfileInvalidException : Exception
{
    public ProfileInvalidException(string message) : base(message) { }
    public ProfileInvalidException() : base() { }
}
