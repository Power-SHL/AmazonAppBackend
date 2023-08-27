namespace AmazonAppBackend.Exceptions.ProfileExceptions;
public class ProfileAlreadyExistsException : Exception
{
    public ProfileAlreadyExistsException(string message) : base(message) { }
    public ProfileAlreadyExistsException() : base() { }
}