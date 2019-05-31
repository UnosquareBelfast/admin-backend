using System;
using System.Linq;
using AdminCore.DTOs.LinkGenerator;
using AdminCore.LinkGenerator.Interfaces;
using HashidsNet;

namespace AdminCore.LinkGenerator.LinkGenerators
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
        /// <param name="timeCreated">Time request was created</param>
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
                EventDateId = eventDateId,
                Salt = salt,
                Hash = new Hashids(salt, HashLength).Encode(eventId),
                TimeCreated =  timeCreated,
                TimeExpires = CalculateRequestExpirationDateTime(timeCreated, requestLifeCycle),
                Expired = false,
                AutoApproved = false
            };
        }

        /// <summary>
        /// todo
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

        public int Decode(string salt, string hash)
        {
            var chunkedEventId = new Hashids(salt, HashLength).Decode(hash);

            if (chunkedEventId.Length == 0)
            {
                throw new Exception($"event Id unknown, did not recognise {hash} request");
            }
            return chunkedEventId.Select((t, i) => t * Convert.ToInt32(Math.Pow(10, chunkedEventId.Length - i - 1))).Sum();
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

        private const int HashLength = 16;
        private const int Day = 24;
    }
}
