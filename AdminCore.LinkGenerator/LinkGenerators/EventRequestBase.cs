using System;
using System.Linq;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.LinkGenerator;
using AdminCore.LinkGenerator.Interfaces;
using HashidsNet;

namespace AdminCore.LinkGenerator.LinkGenerators
{
    public class EventRequestBase : ILinkGenerator
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
        public virtual EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated)
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
                Approved = false,
                AutoApproved = false
            };
        }

        /// <summary>
        /// Generates URL given valid EventRequestDto obj.
        /// </summary>
        /// <param name="eventRequest"></param>
        /// <param name="eventRequestResponse">EventRe</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public HashedLinkDto GenerateLink(EventRequestDto eventRequest, EventRequestResponse eventRequestResponse)
        {
            if (string.IsNullOrEmpty(eventRequest.Hash))
            {
                throw new Exception("Hash value is in invalid format");
            }
            // TODO consider domain, route to be configurable
            return new HashedLinkDto("localhost:8081", $"eventRequest/{(int)eventRequestResponse}", eventRequest.Hash);
        }
        
        /// <summary>
        /// Decodes hash to event Id given hash and salt
        /// </summary>
        /// <param name="salt">Salt value used with to produce Hashid</param>
        /// <param name="hashid">Hashid value</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Decode(string salt, string hashid)
        {
            var chunkedEventId = new Hashids(salt, HashLength).Decode(hashid);

            if (chunkedEventId.Length == 0)
            {
                throw new Exception($"Event Id unknown, did not recognise {hashid} request");
            }
            
            // Hashids decode function returns array of int, converting this array into valid int
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

        private const int Day = 24;
        private const int HashLength = 16;
    }
}
