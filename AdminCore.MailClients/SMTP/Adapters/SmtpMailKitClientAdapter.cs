using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using AdminCore.MailClients.SMTP.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AdminCore.MailClients.SMTP.Adapters
{
    /// <summary>
    /// Implementation of MailKit SMTP client
    /// Connection: SMTP really only uses SSL-wrapped connections on port 465. Port 2 and 587 use StartTLS.
    /// </summary>
    public class SmtpMailKitClientAdapter : ISmtpClient
    {
        private bool _disposed;
        private readonly SmtpClient _smtpClient;
        private readonly IMailServerConfiguration _serverConfiguration;

        public SmtpMailKitClientAdapter(IMailServerConfiguration mailServerConfiguration)
        {
            _serverConfiguration = mailServerConfiguration;
            _smtpClient = new SmtpClient();
            ClientConnectAndAuth();
        }

        public void Connect(string host, int port, bool useSsl,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Connect(host, port, useSsl, cancellationToken);
 
        }

        public Task ConnectAsync(string host, int port, bool useSsl,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.ConnectAsync(host, port, useSsl, cancellationToken);
        }

        public void Connect(string host, int port = 0, CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Connect(host, port, SecureSocketOptions.StartTls, cancellationToken);
        }

        public Task ConnectAsync(string host, int port = 0,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
        }

        public void Connect(Socket socket, string host, int port = 0,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Connect(socket, host, port, SecureSocketOptions.StartTls, cancellationToken);
        }

        public Task ConnectAsync(Socket socket, string host, int port = 0,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.ConnectAsync(socket, host, port, SecureSocketOptions.StartTls, cancellationToken);
        }

        public void Authenticate(ICredentials credentials,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Authenticate(credentials, cancellationToken);
        }

        public Task AuthenticateAsync(ICredentials credentials,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.AuthenticateAsync(credentials, cancellationToken);
        }

        public void Authenticate(Encoding encoding, ICredentials credentials,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Authenticate(encoding, credentials, cancellationToken);
        }

        public Task AuthenticateAsync(Encoding encoding, ICredentials credentials,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.AuthenticateAsync(encoding, credentials, cancellationToken);
        }

        public void Authenticate(Encoding encoding, string userName, string password,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Authenticate(encoding, userName, password, cancellationToken);
        }

        public Task AuthenticateAsync(Encoding encoding, string userName, string password,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.AuthenticateAsync(encoding, userName, password, cancellationToken);
        }

        public void Authenticate(string userName, string password,
            CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Authenticate(userName, password, cancellationToken);
        }

        public Task AuthenticateAsync(string userName, string password,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.AuthenticateAsync(userName, password, cancellationToken);
        }

        public void Disconnect(bool quit, CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Disconnect(quit, cancellationToken);
        }

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.DisconnectAsync(quit, cancellationToken);
        }

        public void NoOp(CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.NoOp(cancellationToken);
        }

        public Task NoOpAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.NoOpAsync(cancellationToken);
        }

        public void Send(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken())
        {
            _smtpClient.Send(GenerateMimeMessage(message), cancellationToken);
        }

        public Task SendAsync(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken())
        {
            return _smtpClient.SendAsync(GenerateMimeMessage(message), cancellationToken);
        }

        /// <summary>
        /// Remove any OAuth functionality as it will not be used
        /// </summary>
        public void ConfigureClient()
        {
            _smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
        }

        public bool IsConnected()
        {
            return _smtpClient.IsConnected;
        }

        public bool IsAuthenticated()
        {
            return _smtpClient.IsAuthenticated;
        }

        private MimeMessage GenerateMimeMessage(MailMessageDto message)
        {
            var mimeMessage = new MimeMessage();

            AddAddresses(mimeMessage.From, message.EmailAddresses.FromAddresses);
            AddAddresses(mimeMessage.To, message.EmailAddresses.ToAddresses);
            AddAddresses(mimeMessage.Cc, message.EmailAddresses.CcAddresses);
            AddAddresses(mimeMessage.Bcc, message.EmailAddresses.BccAddresses);

            mimeMessage.Subject = message.Subject;

            var builder = new BodyBuilder {HtmlBody = message.Content};

            mimeMessage.Body = builder.ToMessageBody();
            return mimeMessage;
        }

        private static void AddAddresses(InternetAddressList addressList, List<EmailAddressDto> addresses)
        {
            addressList.AddRange(addresses.Select(destination =>
                new MailboxAddress(destination.Name, destination.Address)));
        }

        public void Dispose()
        {
            if (_smtpClient == null || !_disposed)
            {
                _smtpClient?.Disconnect(true);
                _smtpClient?.Dispose();
                _disposed = true;
            }
        }

        public void ClientConnectAndAuth()
        {
            var cancellationToken = new CancellationToken();
            if (!IsConnected())
            {
                Connect(_serverConfiguration.ServerAddress, _serverConfiguration.ServerPort, cancellationToken);
            }
            if (!IsAuthenticated())
            {
                Authenticate(_serverConfiguration.ServerUsername, _serverConfiguration.ServerPassword, cancellationToken);
            }
            KeepConnAlive(cancellationToken);
        }

        private void KeepConnAlive(CancellationToken cancellationToken)
        {
            if (IsConnected() && IsAuthenticated())
            {
                Task.Factory.StartNew(() => Watch(cancellationToken), cancellationToken);
            }
        }
        
        private void Watch(CancellationToken cancellationToken = new CancellationToken())
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _smtpClient.NoOp(cancellationToken);
                    Thread.Sleep(_serverConfiguration.SleepKeepConnAliveMs);
                }
                catch (IOException)
                {
                    break;
                }
            }
        }
    }
}
