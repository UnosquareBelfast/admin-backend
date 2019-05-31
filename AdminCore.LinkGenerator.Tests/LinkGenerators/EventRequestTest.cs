using System;
using AdminCore.LinkGenerator.Interfaces;
using AdminCore.LinkGenerator.LinkGenerators;
using AutoFixture;
using AutoFixture.AutoNSubstitute;

using Xunit;

namespace AdminCore.LinkGenerator.Tests.LinkGenerators
{
    public class EventRequestTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnFridayWith48HourLongLifeCycle_IgnoresWeekendExpiresTuesday()
        {
           // Arrange
           const int eventId = 1;
           const int eventDateId = 1;
           const int requestTypeId = 1;
           const int requestLifeCycle = 48;
           var timeCreated = new DateTime(2019, 5, 31); // 2019-05-31 (Friday)

           // Act
           var eventRequest = GetEventRequest();
           var eventRequestDto = eventRequest.CreateRequest(eventId, eventDateId, requestTypeId, requestLifeCycle, timeCreated);
           var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

           // Assert
           Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }
        
        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnSaturdayWith48HourLongLifeCycle_IgnoresSundayExpiresTuesday()
        {
            // Arrange
            const int eventId = 1;
            const int eventDateId = 1;
            const int requestTypeId = 1;
            const int requestLifeCycle = 48;
            var timeCreated = new DateTime(2019, 6, 1); // 2019-06-01 (Saturday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(eventId, eventDateId, requestTypeId, requestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnSundayWith48HourLongLifeCycle_ExpiresTuesday()
        {
            // Arrange
            const int eventId = 1;
            const int eventDateId = 1;
            const int requestTypeId = 1;
            const int requestLifeCycle = 48;
            var timeCreated = new DateTime(2019, 6, 2); // 2019-06-02 (Sunday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(eventId, eventDateId, requestTypeId, requestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 4); // 2019-06-04 (Tuesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnMondayWith48HourLongLifeCycle_ExpiresWednesday()
        {
            // Arrange
            const int eventId = 1;
            const int eventDateId = 1;
            const int requestTypeId = 1;
            const int requestLifeCycle = 48;
            var timeCreated = new DateTime(2019, 6, 3); // 2019-06-03 (Monday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(eventId, eventDateId, requestTypeId, requestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 5); // 2019-06-05 (Wednesday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }
        
        [Fact]
        public void CreateRequest_WhileCreatedEventRequestOnTuesdayWithTwoWeekLongLifeCycle_ExpiresMonday24thOfJune()
        {
            // Arrange
            const int eventId = 1;
            const int eventDateId = 1;
            const int requestTypeId = 1;
            const int requestLifeCycle = 336;
            var timeCreated = new DateTime(2019, 6, 4); // 2019-06-05 (Tuesday)

            // Act
            var eventRequest = GetEventRequest();
            var eventRequestDto = eventRequest.CreateRequest(eventId, eventDateId, requestTypeId, requestLifeCycle, timeCreated);
            var expectedResult = new DateTime(2019, 6, 24); // 2019-06-24 (Monday)

            // Assert
            Assert.Equal(expectedResult, eventRequestDto.TimeExpires);
        }

        private static ILinkGenerator GetEventRequest()
        {
            return new EventRequest();
        }
    }
}
