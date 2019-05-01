using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow.EnumConstants;

namespace AdminCore.FsmWorkflow
{
    public interface IFsmWorkflowHandler
    {
        EventWorkflow CreateEventWorkflow(int eventId, int eventTypeId);

        bool FireLeaveResponded(EventDto employeeEvent, EmployeeDto employee, EventStatuses eventStatus, EventWorkflow eventWorkflow);
    }
}