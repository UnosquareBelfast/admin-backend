using AdminCore.MailClients.Interfaces;

namespace AdminCore.MailClients.SMTP.Configuration
{
    public class SmtpServerConfiguration : IMailServerConfiguration
    {
        public string ServerAddress { get; set; }
        public bool SslEnabled { get; set; }
        public int SleepKeepConnAliveMs { get; set; }
        public int ServerPort { get; set; }
        public string ServerUsername { get; set; }
        public string ServerPassword { get; set; }
    }
}
