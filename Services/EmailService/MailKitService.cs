using AmazonAppBackend.Configuration.Settings;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Storage.EmailStore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Org.BouncyCastle.Cms;

namespace AmazonAppBackend.Services.EmailService;

public class MailKitService : IEmailService
{
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    private readonly string _smtpServer;
    private readonly IEmailStore _emailStore;


    public MailKitService(MailSettings settings, IEmailStore emailStore)
    {
        _senderEmail = settings.SenderEmail;
        _senderPassword = settings.SenderPassword;
        _smtpServer = settings.SmtpServer;
        _emailStore = emailStore;
    }
    public async Task VerifyEmail(UnverifiedProfile recipient)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("StreamIt", _senderEmail));
        message.To.Add(new MailboxAddress(recipient.Username, recipient.Email));
        message.Subject = "Email Verification";

        string content = await _emailStore.GetEmailContent("EmailVerification");
        content = content.Replace("{{recipient_username}}", recipient.Username);
        content = content.Replace("{{verification_code}}", recipient.VerificationCode);

        var builder = new BodyBuilder
        {
            HtmlBody = content
        };

        message.Body = builder.ToMessageBody();
        await SendEmail(message);
    }

    public async Task ResetPasswordEmail(ResetPasswordRequest request)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("StreamIt", _senderEmail));
        message.To.Add(new MailboxAddress(request.Username, request.User.Email));
        message.Subject = "Reset Password";

        string content = await _emailStore.GetEmailContent("ResetPassword");
        content = content.Replace("{{recipient_username}}", request.Username);
        content = content.Replace("{{reset_code}}", request.Code);

        var builder = new BodyBuilder
        {
            HtmlBody = content
        };

        message.Body = builder.ToMessageBody();
        await SendEmail(message);
    }

    private async Task SendEmail(MimeMessage message)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_senderEmail, _senderPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}