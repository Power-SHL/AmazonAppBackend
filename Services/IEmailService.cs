using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services;

public interface IEmailService
{
    Task SendEmail(UnverifiedProfile recipient);
}