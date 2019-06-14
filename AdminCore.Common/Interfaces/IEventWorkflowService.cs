using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DTOs;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventWorkflow;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.Common.Interfaces
{
  public interface IEventWorkflowService
  {
    EventWorkflowDto CreateEventWorkflow(int eventTypeId, bool saveChangesToDbContext = true);
    EventWorkflowDto GetWorkflowByEventId(int eventId);
    IList<SystemUserRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId);
    IDictionary<SystemUserRoleDto, EventStatusDto> GetWorkflowApprovalStatusDictById(int eventId);
    WorkflowFsmStateInfo WorkflowResponse(EventDto employeeEvent, int systemUserId, EventStatuses eventStatus);
  }
}
