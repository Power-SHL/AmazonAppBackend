namespace AmazonAppBackend.Exceptions.ProfileExceptions;
public class ProfileNotFoundException : Exception
{
    public ProfileNotFoundException(string message) : base(message) { }
    public ProfileNotFoundException() : base() { }
}