using FirstResponder.ApplicationCore.Common.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace FirstResponder.Infrastructure.Mail;

public class MailKitService : IMailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailKitService> _logger;

    public MailKitService(IConfiguration configuration, ILogger<MailKitService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public bool SendMail(string toEmail, string toName, string subject, string body)
    {
        try
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["MailSettings:SenderName"],
                _configuration["MailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress(toName, toEmail));

            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(
                _configuration["MailSettings:Server"],
                int.Parse(_configuration["MailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            mailClient.Authenticate(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return false;
        }
    }

    public bool SendMailToMultipleRecipients(Tuple<string, string>[] emailsAndNames, string subject, string body)
    {
        try
        {
            using var emailMessage = new MimeMessage();

            var list = new InternetAddressList();
            
            foreach (var emailAndName in emailsAndNames)
            {
                list.Add(new MailboxAddress(emailAndName.Item2, emailAndName.Item1));
            }
            
            emailMessage.From.Add(new MailboxAddress(_configuration["MailSettings:SenderName"],
                _configuration["MailSettings:SenderEmail"]));
            emailMessage.To.AddRange(list);
            
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(
                _configuration["MailSettings:Server"],
                int.Parse(_configuration["MailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            mailClient.Authenticate(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return false;
        }
    }
}