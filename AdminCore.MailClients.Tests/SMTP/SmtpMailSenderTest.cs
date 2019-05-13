using System.Collections.Generic;
using AdminCore.DTOs.MailMessage;
using AdminCore.MailClients.Interfaces;
using AdminCore.MailClients.SMTP;
using AdminCore.MailClients.SMTP.Interfaces;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using NSubstitute;
using Xunit;

namespace AdminCore.MailClients.Tests.SMTP
{
    public class SmtpMailSenderTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        [Fact]
        public void SendMessages_CallsSendCorrectNoOfTimesGivenListOfMessages()
        {
            // Arrange
            var messageDto = _fixture.Create<MailMessageDto>();

            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            smtpSender.SendMessages(new List<MailMessageDto> {messageDto, messageDto});

            // Assert
            smtpClient.Received(2).Send(Arg.Any<MailMessageDto>());
        }

        [Fact]
        public void SendMessages_CallsSendOncePerEachUniqueMessageGivenListOfMessages()
        {
            // Arrange
            var messageDtoA = _fixture.Create<MailMessageDto>();
            var messageDtoB = _fixture.Create<MailMessageDto>();
            var messageDtoC = _fixture.Create<MailMessageDto>();

            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            smtpSender.SendMessages(new List<MailMessageDto> {messageDtoA, messageDtoB, messageDtoC});

            // Assert
            smtpClient.Received(1).Send(messageDtoA);
            smtpClient.Received(1).Send(messageDtoB);
            smtpClient.Received(1).Send(messageDtoC);
        }

        [Fact]
        public void SendMessages_NoOpsOnSendGivenEmptyListOfMessages()
        {
            // Arrange
            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            smtpSender.SendMessages(new List<MailMessageDto>());

            // Assert
            smtpClient.Received(0).Send(Arg.Any<MailMessageDto>());
        }

        [Fact]
        public void SendMessages_CallsClientConnectAndAuth()
        {
            // Arrange
            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            smtpSender.SendMessages(new List<MailMessageDto>());

            // Assert
            smtpClient.Received(1).ClientConnectAndAuth();
        }

        private static SmtpMailSender GetSmtpMailSender(ISmtpClient client)
        {
            return new SmtpMailSender(client);
        }
    }
}
