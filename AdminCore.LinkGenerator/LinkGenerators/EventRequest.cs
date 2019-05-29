using System;
using AdminCore.DTOs.LinkGenerator;
using HashidsNet;
using RequestLinkGenerator.Interfaces;

namespace RequestLinkGenerator.LinkGenerators
{
    public class EventRequest : ILinkGenerator
    {
        public EventRequestDto CreateRequest(int eventId, int eventDate)
        {
            var eventRequest = new EventRequestDto
            {
                RequestTypeId = 1, EventId = eventId, EventDateId = eventDate, Salt = Guid.NewGuid().ToString("N")
            };

            eventRequest.Hash = new Hashids(eventRequest.Salt).Encode(eventId);
            //todo
            // time created
            // time expires
            eventRequest.Expired = false;
            eventRequest.AutoApproved = false;

            return new EventRequestDto();
        }

        public HashedLinkDto GenerateLink(EventRequestDto eventRequest)
        {
            if (eventRequest.Hash == null)
            {
                throw new Exception("Hash value cannot be null");
            }
            
            // consider domain, route to be configurable
            return new HashedLinkDto("localhost:8081", "event-response", eventRequest.Hash);
        }
    }
}
