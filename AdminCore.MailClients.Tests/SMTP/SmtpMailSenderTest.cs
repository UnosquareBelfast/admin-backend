using System;
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
        public void SendMessages_WithTwoMessageDtoObjectsInList_CallsSendOnSmtpClientTwoTimes()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var messageDto = fixture.Create<MailMessageDto>();

            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            smtpSender.SendMessages(new List<MailMessageDto> {messageDto, messageDto});

            // Assert
            smtpClient.Received(2).Send(Arg.Any<MailMessageDto>());
        }

        [Fact]
        public void SendMessages_WithThreeUniqueMessageDtoObjectsInList_CallsSendOnSmtpClientOnceOnEachMessageDtoObject()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var messageDtoA = fixture.Create<MailMessageDto>();
            var messageDtoB = fixture.Create<MailMessageDto>();
            var messageDtoC = fixture.Create<MailMessageDto>();

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
        public void SendMessages_WithEmptyList_DoesNotInvokeClientConnectAndAuth()
        {
            // Arrange
            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => smtpSender.SendMessages(new List<MailMessageDto>{}));

            // Assert
            Assert.Equal("Messages object cannot be null or empty", ex.Message);
        }

        [Fact]
        public void SendMessages_WithNull_DoesNotInvokeClientConnectAndAuth()
        {
            // Arrange
            var smtpClient = Substitute.For<ISmtpClient>();
            IMailSender smtpSender = GetSmtpMailSender(smtpClient);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => smtpSender.SendMessages(null));

            // Assert
            Assert.Equal("Messages object cannot be null or empty", ex.Message);
        }

        private static SmtpMailSender GetSmtpMailSender(ISmtpClient client)
        {
            return new SmtpMailSender(client);
        }
    }
}
