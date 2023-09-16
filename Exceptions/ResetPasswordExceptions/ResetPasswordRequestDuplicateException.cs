namespace AmazonAppBackend.Exceptions.ResetPasswordExceptions;

public class ResetPasswordRequestDuplicateException : Exception
{
    public ResetPasswordRequestDuplicateException(string message) : base(message) { }
}