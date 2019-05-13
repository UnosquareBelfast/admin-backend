using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.MailMessage;
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
        private readonly SmtpClient _smtpClient = new SmtpClient();
        private readonly IMailServerConfiguration _serverConfiguration;

        public SmtpMailKitClientAdapter(IConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration.RetrieveMailServiceConfig();
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

        public async Task SendAsync(MailMessageDto message, CancellationToken cancellationToken = new CancellationToken())
        {
            await _smtpClient.SendAsync(GenerateMimeMessage(message), cancellationToken);
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

        private static MimeMessage GenerateMimeMessage(MailMessageDto message)
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
            if (!IsConnected())
            {
                Connect(_serverConfiguration.ServerAddress(), _serverConfiguration.ServerPort());
            }

            if (!IsAuthenticated())
            {
                Authenticate(_serverConfiguration.ServerUsername(), _serverConfiguration.ServerPassword());
            }
        }
    }
}
