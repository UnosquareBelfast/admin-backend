namespace AdminCore.MailClients.Interfaces
{
    public interface IMailServerConfiguration
    {
        string ServerAddress { get; set; }
        bool SslEnabled { get; set; }
        int SleepKeepConnAliveMs { get; set; }
        int ServerPort { get; set; }
        string ServerUsername { get; set; }
        string ServerPassword { get; set; }  
    }
}
