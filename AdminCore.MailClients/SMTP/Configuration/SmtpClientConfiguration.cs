using AdminCore.MailClients.Interfaces;

namespace AdminCore.MailClients.SMTP.Configuration
{
    public class SmtpClientConfiguration : ISmtpConfiguration
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
