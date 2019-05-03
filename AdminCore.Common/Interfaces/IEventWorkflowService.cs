using System.Collections.Generic;
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
    IDictionary<EmployeeRoleDto, EventStatusDto> GetWorkflowApprovalStatusDictById(int eventId);
//    bool WorkflowResponseApprove(int eventId, EmployeeDto employee, EventStatuses eventStatus);
    WorkflowFsmStateInfo WorkflowResponseApprove(EventDto employeeEvent, EmployeeDto respondeeEmployee);
    WorkflowFsmStateInfo WorkflowResponseReject(EventDto employeeEvent, EmployeeDto respondeeEmployee);
    WorkflowFsmStateInfo WorkflowResponseCancel(EventDto employeeEvent, EmployeeDto respondeeEmployee);
  }
}