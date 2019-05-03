using System.Collections.Generic;
using AdminCore.DTOs.MailMessage;

namespace AdminCore.MailClients.Interfaces
{
    public interface IMailSender
    {
        void Send(List<MailMessageDto> messages);
        
        void Send(MailMessageDto message);
    }
}