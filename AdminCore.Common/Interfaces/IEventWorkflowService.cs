using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.Common.Interfaces
{
  public interface IEventWorkflowService
  {
    EventWorkflowDto AddEventWorkflow(int eventId, EmployeeDto employee);
    EventWorkflowDto AddEventWorkflow(EventDto employeeEvent, EmployeeDto employee);
    EventWorkflowDto GetWorkflowByEventId(int eventId);
    IList<EmployeeRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId);
    IDictionary<EmployeeRoleDto, ApprovalStatusDto> GetWorkflowApprovalStatusDictById(int eventId);
    EventStatuses UpdateWorkflowResponse(int eventId, EmployeeDto employee, EventStatuses eventStatus);
    EventStatuses UpdateWorkflowResponse(EventDto employeeEvent, EmployeeDto employee, EventStatuses eventStatus);
  }
}