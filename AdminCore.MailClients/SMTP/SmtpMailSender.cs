using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AdminCore.MailClients.SMTP
{
    /// <summary>
    /// Dispatches email messages to mail server using MailKit SMTP Client.
    /// </summary>
    public class SmtpMailSender : ISmtpMailSender
    {
        private readonly IMailServerConfiguration _serverConfiguration;

        public SmtpMailSender(IMailServerConfiguration serverConfiguration)
        {
            _serverConfiguration = serverConfiguration;
        }
        
        /// <summary>
        /// Sends email messages for an event, given authentication was successful. Disconnects from client once an attempt
        /// to send messages was made then disposes of the mail client.
        /// </summary>
        /// <param name="messages">List of messages to dispatch.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <para>username is <c>null</c>.</para>
        /// <para>-or-</para>
        /// <para>password is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:MailKit.MailService" /> has been disposed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// The <see cref="T:MailKit.MailService" /> is not connected or is already authenticated.
        /// </exception>
        /// <exception cref="T:System.OperationCanceledException">
        /// The operation was canceled via the cancellation token.
        /// </exception>
        /// <exception cref="T:MailKit.Security.AuthenticationException">
        /// Authentication using the supplied credentials has failed.
        /// </exception>
        /// <exception cref="T:MailKit.Security.SaslException">
        /// A SASL authentication error occurred.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurred.
        /// </exception>
        /// <exception cref="T:MailKit.ProtocolException">
        /// A protocol error occurred.
        /// </exception>
        public void SendMessages(List<MailMessageDto> messages)
        {
            using (var client = CreateEmailClient())
            {
                client.Authenticate(_serverConfiguration.ServerUsername, _serverConfiguration.ServerPassword);
                messages.ForEach(message => Send(message, client));
                client.Disconnect(true);
            }
        }

        public void Send(MailMessageDto message, SmtpClient client)
        {
            client.Send(GenerateMessage(message));
        }

        private MimeMessage GenerateMessage(MailMessageDto message)
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
            addressList.AddRange(addresses.Select(destination => new MailboxAddress(destination.Name, destination.Address)));
        }

        private SmtpClient CreateEmailClient()
        {
            return ConfigureEmailClient(new SmtpClient());
        }

        private SmtpClient ConfigureEmailClient(SmtpClient client)
        {
            // SMTP really only uses SSL-wrapped connections on port 465
            // Port 2 and 587 use StartTLS
            client.Connect(_serverConfiguration.ServerAddress, _serverConfiguration.ServerPort, SecureSocketOptions.StartTls);

            // Remove any OAuth functionality as it will not be used
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            return client;
        }
    }
}
