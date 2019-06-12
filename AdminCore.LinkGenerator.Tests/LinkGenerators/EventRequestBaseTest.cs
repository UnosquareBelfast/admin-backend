using System;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.LinkGenerator;
using AdminCore.LinkGenerator.Interfaces;
using AdminCore.LinkGenerator.LinkGenerators;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Xunit;

namespace AdminCore.LinkGenerator.Tests.LinkGenerators
{
    public class EventRequestBaseTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnFridayWith48HourLongLifeCycle_IgnoresWeekendExpiresTuesday()
        {
           // Arrange
           var timeCreated = new DateTime(2019, 5, 31); // 2019-05-31 (Friday)

           // Act
           var eventRequest = GetEventRequest();
           var eventRequestDto = eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, RequestLifeCycle, timeCreated);
           var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

           // Assert
           Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }
        
        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnSaturdayWith48HourLongLifeCycle_IgnoresSundayExpiresTuesday()
        {
            // Arrange
            var timeCreated = new DateTime(2019, 6, 1); // 2019-06-01 (Saturday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, RequestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnSundayWith48HourLongLifeCycle_ExpiresTuesday()
        {
            // Arrange
            var timeCreated = new DateTime(2019, 6, 2); // 2019-06-02 (Sunday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, RequestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnMondayWith48HourLongLifeCycle_ExpiresWednesday()
        {
            // Arrange
            var timeCreated = new DateTime(2019, 6, 3); // 2019-06-03 (Monday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, RequestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 5); // 2019-06-05 (Wednesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }
        
        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnTuesdayWithTwoWeekLongLifeCycle_ExpiresMonday24thOfJune()
        {
            // Arrange
            const int requestLifeCycle = 336;
            var timeCreated = new DateTime(2019, 6, 4); // 2019-06-05 (Tuesday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, requestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 24); // 2019-06-24 (Monday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        [Fact]
        public void CreateRequest_WithInvalidRequestLifeCycleValue_ThrowsException()
        {
            // Arrange
            const int requestLifeCycle = -1;
            var timeCreated = new DateTime(2019, 6, 4); // 2019-06-05 (Tuesday)

            // Act
            var eventRequest = GetEventRequest();
            var ex = Assert.Throws<Exception>(() => eventRequest.CreateRequest(EventId, EventDateId, RequestTypeId, requestLifeCycle, timeCreated));

            // Assert
            Assert.Equal($"Request Life Cycle value is invalid, value={requestLifeCycle}", ex.Message);
        }

        [Fact]
        public void GenerateLink_WithNullHashValue_ThrowsException()
        {
            // Arrange
            var eventRequestDto = _fixture.Create<EventRequestDto>();
            eventRequestDto.Hash = null;

            // Act
            var eventRequest = GetEventRequest();
            var ex = Assert.Throws<Exception>(() => eventRequest.GenerateLink(eventRequestDto, EventRequestResponse.Accept));

            // Assert
            Assert.Equal($"Hash value is in invalid format", ex.Message);
        }

        [Fact]
        public void GenerateLink_WithEmptyHashValue_ThrowsException()
        {
            // Arrange
            var eventRequestDto = _fixture.Create<EventRequestDto>();
            eventRequestDto.Hash = "";

            // Act
            var eventRequest = GetEventRequest();
            var ex = Assert.Throws<Exception>(() => eventRequest.GenerateLink(eventRequestDto, EventRequestResponse.Accept));

            // Assert
            Assert.Equal($"Hash value is in invalid format", ex.Message);
        }

        [Fact]
        public void GenerateLink_WithHashValueAndAcceptEventRequestResponseType_BuildsValidLink()
        {
            // Arrange
            var eventRequestDto = _fixture.Create<EventRequestDto>();

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestLink = eventRequest.GenerateLink(eventRequestDto, EventRequestResponse.Accept);

            // Assert
            Assert.Equal($"https://localhost:8081/eventRequest/0/{eventRequestDto.Hash}", eventRequestLink.HashedLink);
        }

        [Fact]
        public void GenerateLink_WithHashValueAndRejectEventRequestResponseType_BuildsValidLink()
        {
            // Arrange
            var eventRequestDto = _fixture.Create<EventRequestDto>();

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestLink = eventRequest.GenerateLink(eventRequestDto, EventRequestResponse.Reject);

            // Assert
            Assert.Equal($"https://localhost:8081/eventRequest/1/{eventRequestDto.Hash}", eventRequestLink.HashedLink);
        }

        [Fact]
        public void Decode_WithIncorrectHashAndSalt_FailsToDecodeAndThrowsException()
        {
            // Arrange
            const string salt = "salt";
            const string hash = "hash";

            // Act
            var eventRequest = GetEventRequest();
            var ex = Assert.Throws<Exception>(() => eventRequest.Decode(salt, hash));

            // Assert
            Assert.Equal($"Event Id unknown, did not recognise {hash} request", ex.Message);
        }

        [Fact]
        public void Decode_WithCorrectHashAndSalt_ConvertsArrayOfIntIntoValidEventId()
        {
            // Arrange
            const string salt = "salt";
            const string hash = "qpNKDE3P182OBdrQ";

            // Act
            var eventRequest = GetEventRequest();
            var eventId = eventRequest.Decode(salt, hash);

            // Assert
            Assert.Equal(15, eventId);
        }

        private static ILinkGenerator GetEventRequest()
        {
            return new EventRequestBase();
        }

        private const int EventId = 1;
        private const int EventDateId = 1;
        private const int RequestTypeId = 1;
        private const int RequestLifeCycle = 48;
    }
}
