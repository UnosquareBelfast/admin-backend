using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.SMTP.Interfaces;

namespace AdminCore.MailClients.Interfaces
{
    public interface IMailSender
    {
        void SendMessages(List<MailMessageDto> messages);
    }
}
