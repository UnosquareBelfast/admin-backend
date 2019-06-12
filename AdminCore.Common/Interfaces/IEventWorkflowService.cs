using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DTOs;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.Common.Interfaces
{
  public interface IEventWorkflowService
  {
    EventWorkflowDto CreateEventWorkflow(int eventTypeId, bool saveChangesToDbContext = true);
    EventWorkflowDto GetWorkflowByEventId(int eventId);
    IList<EmployeeRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId);
    IDictionary<EmployeeRoleDto, EventStatusDto> GetWorkflowApprovalStatusDictById(int eventId);
    WorkflowFsmStateInfo WorkflowResponse(EventDto employeeEvent, SystemUserDto respondeeSystemUser, EventStatuses eventStatus);
  }
}
