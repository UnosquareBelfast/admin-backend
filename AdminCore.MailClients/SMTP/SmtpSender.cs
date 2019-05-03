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
        private readonly ISmtpConfiguration _smtpConfiguration;

        public SmtpSender(ISmtpConfiguration smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration;
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
