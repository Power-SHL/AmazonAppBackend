namespace AmazonAppBackend.Exceptions.ProfileExceptions;

public class ProfileVerificationException : Exception
{
    public ProfileVerificationException(string message) : base(message) { }
}
