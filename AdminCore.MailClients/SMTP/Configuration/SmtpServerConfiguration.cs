using System;
using AdminCore.Common.Interfaces;

namespace AdminCore.MailClients.SMTP.Configuration
{
    public class SmtpServerConfiguration : IMailServerConfiguration
    {
        public string ServerAddress()
        {
            return Environment.GetEnvironmentVariable("SERV_ADDRESS")
                   ?? "smtp.office365.com";
        }

        public bool SslEnabled()
        {
            return string.Equals(Environment.GetEnvironmentVariable("SSL_ENABLED"), "true",
                StringComparison.CurrentCultureIgnoreCase);
        }

        public int ServerPort()
        {
            return (int.TryParse(Environment.GetEnvironmentVariable("SERV_PORT"), out var v) ? v : default(int?))
                   ?? 587;
        }

        public string ServerUsername()
        {
            return Environment.GetEnvironmentVariable("SERV_USERNAME")
                   ?? "user";
        }

        public string ServerPassword()
        {
            return Environment.GetEnvironmentVariable("SERV_PASSWORD")
                   ?? "password";
        }
    }
}
