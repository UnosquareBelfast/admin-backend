using System;
using System.Text;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.LinkGenerator;

namespace AdminCore.LinkGenerator.Interfaces
{
    public interface ILinkGenerator
    {
        EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated);
        HashedLinkDto GenerateLink(EventRequestDto eventRequestDto, EventRequestResponse eventRequestResponse);
        int Decode(string salt, string hash);
    }
}
