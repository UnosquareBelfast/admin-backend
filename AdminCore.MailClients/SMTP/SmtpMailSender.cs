using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using AdminCore.MailClients.SMTP.Interfaces;

namespace AdminCore.MailClients.SMTP
{
    /// <summary>
    /// Dispatches email messages to mail server using an SMTP Client.
    /// </summary>
    public class SmtpMailSender : IMailSender
    {
        private readonly ISmtpClient _smtpClient;

        public SmtpMailSender(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public void SendMessages(List<MailMessageDto> messages)
        {
            if (messages?.Any() == true)
            {
                // authentication per each request is slow and it is a temporary solution,
                // this will change once smtp impl is moved to its own micro-service
                _smtpClient.ClientConnectAndAuth();

                Task
                    .Run(() => messages.ForEach(message => _smtpClient.Send(message)))
                    .Wait();
            }
            else
            {
                throw new InvalidOperationException("Messages object cannot be null or empty");
            }
        }
    }
}
