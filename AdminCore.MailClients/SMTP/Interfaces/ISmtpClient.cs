using System;
using System.Threading;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;

namespace AdminCore.MailClients.SMTP.Interfaces
{
    public interface ISmtpClient : IDisposable
    {
        void Connect(string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        Task ConnectAsync(string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        void Authenticate(string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        void ClientConnectAndAuth();

        void Disconnect(bool quit, CancellationToken cancellationToken = new CancellationToken());

        Task DisconnectAsync(bool quit, CancellationToken cancellationToken = new CancellationToken());

        void Send(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken());

        Task SendAsync(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken());
        
        void ConfigureClient();

        bool IsConnected();

        bool IsAuthenticated();

        bool Disposed { get; set; }
    }
}
