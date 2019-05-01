using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.Common.Interfaces
{
  public interface IEventWorkflowService
  {
    EventWorkflowDto CreateEventWorkflow(int eventId, int eventTypeId);
    EventWorkflowDto GetWorkflowByEventId(int eventId);
    IList<EmployeeRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId);
    IDictionary<EmployeeRoleDto, ApprovalStatusDto> GetWorkflowApprovalStatusDictById(int eventId);
//    bool WorkflowResponseApprove(int eventId, EmployeeDto employee, EventStatuses eventStatus);
    bool WorkflowResponseApprove(EventDto employeeEvent, EmployeeDto employee);
    bool WorkflowResponseReject(EventDto employeeEvent, EmployeeDto employee);
    bool WorkflowResponseCancel(EventDto employeeEvent, EmployeeDto employee);
  }
}