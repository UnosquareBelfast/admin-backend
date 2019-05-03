using System.Collections.Generic;
using AdminCore.DTOs.MailMessage;

namespace AdminCore.MailClients.Interfaces
{
    public interface IMailSender
    {
        void SendMessages(List<MailMessageDto> messages);
    }
}
