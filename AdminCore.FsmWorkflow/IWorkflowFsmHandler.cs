using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;

namespace AdminCore.FsmWorkflow
{
    public interface IWorkflowFsmHandler
    {
        EventWorkflow CreateEventWorkflow(int eventTypeId, bool saveChangesToDbContext);

        WorkflowFsmStateInfo FireLeaveResponse(EventDto employeeEvent, SystemUserDto respondeeSystemUser, EmployeeRoles employeeRole, EventStatuses eventStatus, EventWorkflow eventWorkflow);
    }
}