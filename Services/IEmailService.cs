using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services;

public interface IEmailService
{
    Task SendEmail(Profile recipient);
}