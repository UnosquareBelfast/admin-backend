using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AdminCore.MailClients.SMTP
{
    /// <summary>
    /// Dispatches email messages to mail server using MailKit SMTP Client.
    /// </summary>
    public class SmtpMailSender : IMailSender, ISmtpMailSender
    {
        private readonly IMailServerConfiguration _serverConfiguration;

        public SmtpMailSender(IMailServerConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration;
        }
        
        public void SendMessages(List<MailMessageDto> messages)
        {
            using (var client = CreateEmailClient())
            {
                client.Authenticate(_serverConfiguration.ServerUsername, _serverConfiguration.ServerPassword);
                messages.ForEach(message => Send(message, client));
                client.Disconnect(true);
            }
        }

        public void Send(MailMessageDto message, SmtpClient client)
        {
            client.Send(GenerateMessage(message));
        }

        private MimeMessage GenerateMessage(MailMessageDto message)
        {
            var mimeMessage = new MimeMessage();
   
            AddSenders(mimeMessage, message.EmailAddresses.FromAddresses);
            AddRecipients(mimeMessage, message.EmailAddresses.ToAddresses);
            AddRecipients(mimeMessage, message.EmailAddresses.CcAddresses);
            AddRecipients(mimeMessage, message.EmailAddresses.BccAddresses);

            mimeMessage.Subject = message.Subject;

            var builder = new BodyBuilder {HtmlBody = message.Content};

            mimeMessage.Body = builder.ToMessageBody();
            return mimeMessage;
        }

        private void AddSenders(MimeMessage mimeMessage, List<EmailAddressDto> addresses)
        {
            mimeMessage.From.AddRange(addresses.Select(destination => new MailboxAddress(destination.Name, destination.Address)));
        }
        
        private void AddRecipients(MimeMessage mimeMessage, List<EmailAddressDto> addresses)
        {
            mimeMessage.To.AddRange(addresses.Select(destination => new MailboxAddress(destination.Name, destination.Address)));
        }

        private SmtpClient CreateEmailClient()
        {
            return ConfigureEmailClient(new SmtpClient());
        }

        private SmtpClient ConfigureEmailClient(SmtpClient client)
        {
            // SMTP really only uses SSL-wrapped connections on port 465
            // Port 2 and 587 use StartTLS
            client.Connect(_serverConfiguration.ServerAddress, _serverConfiguration.ServerPort, SecureSocketOptions.StartTls);
            // Remove any OAuth functionality as it will not be used
            client.AuthenticationMechanisms.Remove("XOAUTH2");            
            return client;
        }
    }
}
