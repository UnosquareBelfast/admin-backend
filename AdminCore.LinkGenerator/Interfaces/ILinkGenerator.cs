using System;
using AdminCore.DTOs.LinkGenerator;

namespace RequestLinkGenerator.Interfaces
{
    public interface ILinkGenerator
    {
        EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated);
        HashedLinkDto GenerateLink(EventRequestDto eventRequestDto);
    }
}
