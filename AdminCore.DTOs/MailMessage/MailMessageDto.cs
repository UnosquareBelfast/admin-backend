using System.Collections.Generic;

namespace AdminCore.DTOs.MailMessage
{
    public class MailMessageDto
    {
        public MailMessageDto()
        {
            ToAddresses = new List<EmailAddressDto>();
            FromAddresses = new List<EmailAddressDto>();
        }
        
        public List<EmailAddressDto> ToAddresses { get; set; }
        public List<EmailAddressDto> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
