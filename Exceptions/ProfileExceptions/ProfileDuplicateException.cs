namespace AmazonAppBackend.Exceptions.ProfileExceptions;
public class ProfileDuplicateException : Exception
{
    public ProfileDuplicateException(string message) : base(message) { }
    public ProfileDuplicateException() : base() { }
}