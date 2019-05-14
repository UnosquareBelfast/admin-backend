namespace AdminCore.DTOs.MailMessage
{
    public class EmailAddressDto
    {
        public EmailAddressDto(string name, string address)
        {
            Name = name;
            Address = address;
        }
        public string Name { get; }
        public string Address { get; }
    }
}
