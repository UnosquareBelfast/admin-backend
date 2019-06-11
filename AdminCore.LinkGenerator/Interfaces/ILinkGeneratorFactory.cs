using AdminCore.Constants.Enums;

namespace AdminCore.LinkGenerator.Interfaces
{
    public interface ILinkGeneratorFactory
    {
        ILinkGenerator Create(EventRequestTypes type);
    }
}
