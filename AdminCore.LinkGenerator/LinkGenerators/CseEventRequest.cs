using System;
using AdminCore.DTOs.LinkGenerator;

namespace AdminCore.LinkGenerator.LinkGenerators
{
    public class CseEventRequest : EventRequestBase
    {
        public override EventRequestDto CreateRequest(int eventId, int eventDateId, int requestTypeId, int requestLifeCycle, DateTime timeCreated)
        {
            throw new NotImplementedException();
        }
    }
}
