using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using AdminCore.MailClients.SMTP.Interfaces;

namespace AdminCore.MailClients.SMTP
{
    /// <summary>
    /// Dispatches email messages to mail server using an SMTP Client.
    /// </summary>
    public class SmtpMailSender : IMailSender
    {
        private readonly ISmtpClient _smtpClient;

        public SmtpMailSender(ISmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
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
        public async void SendMessages(List<MailMessageDto> messages)
        {
            _smtpClient.ClientConnectAndAuth();
            await Task.WhenAll(messages.Select(message => _smtpClient.SendAsync(message)));
        }
    }
}
