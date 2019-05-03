using AdminCore.DTOs.MailMessage;
using MailKit.Net.Smtp;

namespace AdminCore.MailClients.Interfaces
{
    public interface ISmtpMailSender : IMailSender
    {
        void Send(MailMessageDto message, SmtpClient client);
    }
}
