using System;
using AdminCore.DTOs.LinkGenerator;
using HashidsNet;
using RequestLinkGenerator.Interfaces;

namespace RequestLinkGenerator.LinkGenerators
{
    /// <summary>
    /// Creates Event Request obj and generates link for that obj
    /// </summary>
    public class EventRequest : ILinkGenerator
    {
        /// <summary>
        /// Creates Event Request while ensuring expiration date time is calculated considering only business days.
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="eventDateId">Event Date Id</param>
        /// <param name="requestTypeId">Request Type Id</param>
        /// <param name="requestLifeCycle">Request Life Cycle, expected multiple of 24</param>
        /// <returns>EventRequestDto obj</returns>
        /// <exception cref="Exception">Is thrown given invalid requestLifeCycle</exception>
        public EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated)
        {
            if (requestLifeCycle % Day != 0)
            {
                throw new Exception($"Request Life Cycle value is invalid, value={requestLifeCycle}");
            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
