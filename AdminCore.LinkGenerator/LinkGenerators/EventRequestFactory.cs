using System;
using AdminCore.Constants.Enums;
using AdminCore.LinkGenerator.Interfaces;

namespace AdminCore.LinkGenerator.LinkGenerators
{
    public class EventRequestFactory : ILinkGeneratorFactory
    {
        public ILinkGenerator Create(EventRequestTypes type)
        {
            switch (type)
            {
                case EventRequestTypes.ClientPtoRequest:
                case EventRequestTypes.HrPtoRequest:
                    return new EventRequestBase();
                case EventRequestTypes.TeamLeadPtoRequest:
                    return new TeamLeadEventRequest();
                case EventRequestTypes.CsePtoRequest:
                    return new CseEventRequest();
                default:
                    throw new Exception($"{type} is either unknown or invalid Event Request Type");
            }
        }
    }
}
