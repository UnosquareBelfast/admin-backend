using System.Collections.Generic;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;

namespace AdminCore.MailClients.SMTP
{
    /// <summary>
    /// Dispatches email messages to mail server using MailKit SMTP Client.
    /// </summary>
    public class SmtpSender : IMailSender
    {
        private readonly IMailServerConfiguration _mailServerConfiguration;

        public SmtpSender(IMailServerConfiguration mailServerConfiguration)
        {
            _mailServerConfiguration = mailServerConfiguration;
        }
        
        public void Send(List<MailMessageDto> messages)
        {
            //todo implementation
        }

        public void Send(MailMessageDto message)
        {
            //todo implementation
        }
    }
}
