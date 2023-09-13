using AmazonAppBackend.Configuration;
using AmazonAppBackend.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AmazonAppBackend.Services;

public class GmailService : IEmailService
{
    private readonly string _senderEmail;
    private readonly string _senderPassword;

    public GmailService(GmailSettings settings)
    {
        _senderEmail = settings.SenderEmail;
        _senderPassword = settings.SenderPassword;
    }
    public async Task SendEmail(Profile recipient)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("SocialSync", _senderEmail));
        message.To.Add(new MailboxAddress(recipient.Username, recipient.Email));
        message.Subject = "EmailVerification";

        var builder = new BodyBuilder
        {
            HtmlBody = $"<br>Hi {recipient.Username}! Verify your account"
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient() ;
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_senderEmail, _senderPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}