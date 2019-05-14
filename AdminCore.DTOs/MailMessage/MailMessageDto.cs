namespace AdminCore.DTOs.MailMessage
{
    public class MailMessageDto
    {
        public MailMessageDto(EmailAddressesDto emailAddress, string subject, string content)
        {
            EmailAddresses = emailAddress;
            Subject = subject;
            Content = content;
        }
        
        public EmailAddressesDto EmailAddresses { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
