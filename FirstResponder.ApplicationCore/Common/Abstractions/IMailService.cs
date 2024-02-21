namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IMailService
{
    bool SendMail(string toEmail, string toName, string subject, string body);
}