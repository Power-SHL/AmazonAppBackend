using AmazonAppBackend.Configuration;
using AmazonAppBackend.DTO;
using AmazonAppBackend.Storage.EmailStore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AmazonAppBackend.Services;

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
    public async Task SendEmail(UnverifiedProfile recipient)
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
        using var client = new SmtpClient() ;
        await client.ConnectAsync(_smtpServer, 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_senderEmail, _senderPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}