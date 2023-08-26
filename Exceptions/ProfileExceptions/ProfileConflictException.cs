namespace AmazonAppBackend.Exceptions.ProfileExceptions;
public class ProfileConflictException : Exception
{
    public ProfileConflictException(string message) : base(message) { }
    public ProfileConflictException() : base() { }
}