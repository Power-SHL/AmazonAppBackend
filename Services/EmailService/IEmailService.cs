using AmazonAppBackend.DTO;

namespace AmazonAppBackend.Services.EmailService;

public interface IEmailService
{
    Task VerifyEmail(UnverifiedProfile recipient);
    Task ResetPasswordEmail(ResetPasswordRequest request);
}