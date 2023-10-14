using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using AuthSystem.core.Interfaces.Email;
using AuthSystem.Infrastructure.Helpers;

namespace AuthSystem.Services.Impentation.Email;





public class EmailService : IEmailService
{
    private readonly AppSettings _appSettings;

    public EmailService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

   
    public void Send(string to, string subject, string html, string from = null)
    {
        // create message
        var email = new MimeMessage();
        //email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
        email.From.Add(new MailboxAddress("AuthSystem", _appSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;

        //var bodyBuilder = new BodyBuilder();
        //bodyBuilder.HtmlBody = html;
        //email.Body = bodyBuilder.ToMessageBody();
        email.Body = new TextPart(TextFormat.Html) { Text = html };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, true);
        smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}