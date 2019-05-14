using System.Threading;
using AdminCore.Common.Interfaces;
using AdminCore.MailClients.SMTP.Adapters;
using AdminCore.MailClients.SMTP.Interfaces;
using MailKit;
using MailKit.Security;
using NSubstitute;
using Xunit;

namespace AdminCore.MailClients.Tests.SMTP.Adapters
{
    public class SmtpMailKitClientAdapterTest
    {
        private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
        private static readonly IMailServerConfiguration ServerConfiguration = Substitute.For<IMailServerConfiguration>();

        [Fact]
        public void Dispose_WithDisposedPropertyAsTrue_DoesNotDisconnectFromSmtpClientAndDisposeOfResources()
        {
            // Arrange
            var smtpClient = Substitute.For<IMailTransport>();
            var adapter = GetClientAdapter(smtpClient, true);
            Configuration.RetrieveMailServiceConfig().Returns(ServerConfiguration);

            // Act
            adapter.Dispose();

            // Assert
            smtpClient.Received(0).Disconnect(true);
            smtpClient.Received(0).Dispose();
        }

        [Fact]
        public void Dispose_WithDisposedPropertyAsFalse_DisconnectsFromSmtpClientAndDisposesOfResources()
        {
            // Arrange
            var smtpClient = Substitute.For<IMailTransport>();
            var adapter = GetClientAdapter(smtpClient, false);
            Configuration.RetrieveMailServiceConfig().Returns(ServerConfiguration);

            // Act
            adapter.Dispose();

            // Assert
            smtpClient.Received(1).Disconnect(true);
            smtpClient.Received(1).Dispose();
            Assert.Equal(true, adapter.Disposed);
        }

        [Fact]
        public void ClientConnectAndAuth_WithIsConnectedAndIsAuthenticatedPropertyAsTrue_DoesNotConnectAndAuthenticateWithSmtpClient()
        {
            // Arrange
            var smtpClient = Substitute.For<IMailTransport>();
            var adapter = GetClientAdapter(smtpClient, false);
            Configuration.RetrieveMailServiceConfig().Returns(ServerConfiguration);

            smtpClient.IsConnected.Returns(true);
            smtpClient.IsAuthenticated.Returns(true);

            // Act
            adapter.ClientConnectAndAuth();

            // Assert
            smtpClient.Received(0).Connect(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<SecureSocketOptions>(), Arg.Any<CancellationToken>());
            smtpClient.Received(0).Authenticate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public void ClientConnectAndAuth_WithIsConnectedAndIsAuthenticatedPropertyAsFalse_ConnectsAndAuthenticatesWithSmtpClient()
        {
            // Arrange
            var smtpClient = Substitute.For<IMailTransport>();
            var adapter = GetClientAdapter(smtpClient, false);
            Configuration.RetrieveMailServiceConfig().Returns(ServerConfiguration);

            smtpClient.IsConnected.Returns(false);
            smtpClient.IsAuthenticated.Returns(false);

            // Act
            adapter.ClientConnectAndAuth();

            // Assert
            smtpClient.Received(1).Connect(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<SecureSocketOptions>(), Arg.Any<CancellationToken>());
            smtpClient.Received(1).Authenticate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public void ClientConnectAndAuth_WithIsAuthenticatedPropertyAsTrue_DoesNotConnectAndAuthenticateWithSmtpClient()
        {
            // Arrange
            var smtpClient = Substitute.For<IMailTransport>();
            var adapter = GetClientAdapter(smtpClient, false);
            Configuration.RetrieveMailServiceConfig().Returns(ServerConfiguration);

            smtpClient.IsAuthenticated.Returns(true);

            // Act
            adapter.ClientConnectAndAuth();

            // Assert
            smtpClient.Received(0).Connect(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<SecureSocketOptions>(), Arg.Any<CancellationToken>());
            smtpClient.Received(0).Authenticate(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

        private static ISmtpClient GetClientAdapter(IMailTransport client, bool disposed)
        {
            ISmtpClient clientAdapter = new SmtpMailKitClientAdapter(Configuration, client);
            clientAdapter.Disposed = disposed;
            return clientAdapter;
        }
    }
}
