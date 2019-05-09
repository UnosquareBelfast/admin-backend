using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;
using MailKit.Net.Smtp;

namespace AdminCore.MailClients.SMTP.Interfaces
{
    public interface ISmtpClient : IDisposable
    {
        void Connect(string host, int port, bool useSsl, CancellationToken cancellationToken = new CancellationToken());

        Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = new CancellationToken());

        void Connect(string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        Task ConnectAsync(string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        void Connect(Socket socket, string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        Task ConnectAsync(Socket socket, string host, int port = 0, CancellationToken cancellationToken = new CancellationToken());

        void Authenticate(ICredentials credentials, CancellationToken cancellationToken = new CancellationToken());

        Task AuthenticateAsync(ICredentials credentials, CancellationToken cancellationToken = new CancellationToken());

        void Authenticate(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = new CancellationToken());

        Task AuthenticateAsync(Encoding encoding, ICredentials credentials, CancellationToken cancellationToken = new CancellationToken());

        void Authenticate(Encoding encoding, string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        Task AuthenticateAsync(Encoding encoding, string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        void Authenticate(string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = new CancellationToken());

        void ClientConnectAndAuth();

        void Disconnect(bool quit, CancellationToken cancellationToken = new CancellationToken());

        Task DisconnectAsync(bool quit, CancellationToken cancellationToken = new CancellationToken());

        void NoOp(CancellationToken cancellationToken = new CancellationToken());

        Task NoOpAsync(CancellationToken cancellationToken = new CancellationToken());

        void Send(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken());

        Task SendAsync(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken());
        
        void ConfigureClient();

        bool IsConnected();

        bool IsAuthenticated();
    }
}
