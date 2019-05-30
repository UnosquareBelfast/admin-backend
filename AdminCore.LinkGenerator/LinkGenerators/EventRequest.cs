using System;
using AdminCore.DTOs.LinkGenerator;
using HashidsNet;
using RequestLinkGenerator.Interfaces;

namespace RequestLinkGenerator.LinkGenerators
{
    public class EventRequest : ILinkGenerator
    {
        public EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated)
        {
            var salt = Guid.NewGuid().ToString("N");
            return new EventRequestDto
            {
                RequestTypeId = requestTypeId,
                EventId = eventId,
                EventDateId = eventDateId,
                Salt = salt,
                Hash = new Hashids(salt).Encode(eventId),
                TimeCreated =  timeCreated,
                TimeExpires = CalculateRequestExpirationDateTime(timeCreated, requestLifeCycle),
                Expired = false,
                AutoApproved = false
            };
        }

        public HashedLinkDto GenerateLink(EventRequestDto eventRequest)
        {
            if (eventRequest.Hash == null)
            {
                throw new Exception("Hash value cannot be null");
            }

            // todo consider domain, route to be configurable
            return new HashedLinkDto("localhost:8081", "event-response", eventRequest.Hash);
        }

        private static DateTime CalculateRequestExpirationDateTime(DateTime timeCreated, int requestLifeCycle)
        {
            var days = requestLifeCycle / Day;
            var nextDay = timeCreated.AddHours(Day);

            for (var i = 0; i < days;)
            {
                if (nextDay.DayOfWeek == DayOfWeek.Saturday || nextDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    nextDay = nextDay.AddHours(Day);
                    continue;
                }
                timeCreated = nextDay;

                nextDay = nextDay.AddHours(Day);
                i++;
            }
            return timeCreated;
        }
        private const int Day = 24;
    }
}
