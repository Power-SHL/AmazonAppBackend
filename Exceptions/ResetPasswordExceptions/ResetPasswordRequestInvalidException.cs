namespace AmazonAppBackend.Exceptions.ResetPasswordExceptions;

public class ResetPasswordRequestInvalidException : Exception
{
    public ResetPasswordRequestInvalidException(string message) : base(message) { }
}