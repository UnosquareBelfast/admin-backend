using System.Collections.Generic;
using System.Linq;

namespace AdminCore.DTOs.MailMessage
{
    public class EmailAddressesDto
    {
        public EmailAddressesDto(
            List<EmailAddressDto> fromAddresses,
            List<EmailAddressDto> toAddresses,
            List<EmailAddressDto> ccAddresses = null,
            List<EmailAddressDto> bccAddresses = null)
        {
            FromAddresses = fromAddresses;
            ToAddresses = toAddresses;
            CcAddresses = ccAddresses ?? new List<EmailAddressDto>();
            BccAddresses = bccAddresses ?? new List<EmailAddressDto>();
        }
        
        public List<EmailAddressDto> FromAddresses { get; }
        public List<EmailAddressDto> ToAddresses { get; }
        public List<EmailAddressDto> CcAddresses { get; }
        public List<EmailAddressDto> BccAddresses { get; }
    }
}
