using AdminCore.DTOs.LinkGenerator;

namespace RequestLinkGenerator.Interfaces
{
    public interface ILinkGenerator
    {
        EventRequestDto CreateRequest(int eventId, int eventDate);
        HashedLinkDto GenerateLink(EventRequestDto eventRequestDto);
    }
}
