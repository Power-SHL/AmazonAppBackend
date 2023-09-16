namespace AmazonAppBackend.Exceptions.ResetPasswordExceptions;

public class ResetPasswordRequestNotFoundException : Exception
{
    public ResetPasswordRequestNotFoundException(string message) : base(message) { }
}