namespace AdminCore.Common.Interfaces
{
    public interface IMailServerConfiguration
    {
        string ServerAddress();
        bool SslEnabled();
        int ServerPort();
        string ServerUsername();
        string ServerPassword();
    }
}
