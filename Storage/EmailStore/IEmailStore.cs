namespace AmazonAppBackend.Storage.EmailStore;

public interface IEmailStore
{
    Task<string> GetEmailContent(string emailType);
}