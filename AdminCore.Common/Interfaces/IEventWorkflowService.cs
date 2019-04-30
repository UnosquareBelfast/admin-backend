using AdminCore.Constants.Enums;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.Common.Interfaces
{
  public interface IEventWorkflowService
  {
    EventWorkflowDto CreateEventWorkflow(int eventId, EventTypes eventTypes);
  }
}