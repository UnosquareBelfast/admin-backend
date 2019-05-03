using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow.FsmMachines;

namespace AdminCore.FsmWorkflow
{
    public interface IFsmWorkflowHandler
    {
        EventWorkflow CreateEventWorkflow(int eventId, int eventTypeId);

        WorkflowFsmStateInfo FireLeaveResponse(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatus, EventWorkflow eventWorkflow);
    }
}