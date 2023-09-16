namespace AmazonAppBackend.Exceptions.ProfileExceptions;

public class ResetPasswordRequestDuplicateException : Exception
{
    public ResetPasswordRequestDuplicateException(string message) : base(message) { }
}