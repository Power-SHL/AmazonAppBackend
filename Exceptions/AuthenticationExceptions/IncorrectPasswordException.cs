namespace AmazonAppBackend.Exceptions.AuthenticationExceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string message) : base(message) {}
}
