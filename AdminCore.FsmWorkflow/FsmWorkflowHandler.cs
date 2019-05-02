using System.Collections.Generic;
using System.Linq;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow.FsmMachines;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;

namespace AdminCore.FsmWorkflow
{
    public class FsmWorkflowHandler : IFsmWorkflowHandler
    {
        private readonly IDatabaseContext _dbContext;
        public FsmWorkflowHandler(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public EventWorkflow CreateEventWorkflow(int eventId, int eventTypeId)
        {
            var eventWorkflow = new EventWorkflow
            {
                EventId = eventId,
                WorkflowState = GetInitialWorkflowState(eventTypeId)
            };

            eventWorkflow.EventWorkflowResponders = CreateEventWorkflowResponders(eventTypeId, eventWorkflow);
            
            return eventWorkflow;
        }

        public bool FireLeaveResponse(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatus, EventWorkflow eventWorkflow)
        {
            switch (employeeEvent.EventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    var approvalDict = RebuildApprovalDictionary(employeeEvent.EventTypeId, eventWorkflow);
                    
                    var workflowStateData = new WorkflowStatePto((PtoState)eventWorkflow.WorkflowState, approvalDict);
                    var workflowFsm = new WorkflowFsmPto(workflowStateData);
                    
                    // Cast is not redundant, need the int value of enum as a string
                    // ReSharper disable once RedundantCast
                    return workflowFsm.FireLeaveResponded(eventStatus, ((int)respondeeEmployee.EmployeeRoleId).ToString());
                default:
                    return false;
            }
        }

        private Dictionary<string, EventStatuses> RebuildApprovalDictionary(int eventTypeId, EventWorkflow eventWorkflow)
        {
            switch (eventTypeId)
            {
                case (int) EventTypes.AnnualLeave:
                    var teamLeadLastResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.TeamLeader)?.EventStatusId
                                               ?? (int)EventStatuses.AwaitingApproval;
                    var clientResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Client)?.EventStatusId
                                             ?? (int)EventStatuses.AwaitingApproval;
                    var cseResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Cse)?.EventStatusId 
                                          ?? (int)EventStatuses.AwaitingApproval;
                    var adminResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.SystemAdministrator)?.EventStatusId 
                                          ?? (int)EventStatuses.AwaitingApproval;

                    return new Dictionary<string, EventStatuses>
                    {
                        {((int) EmployeeRoles.TeamLeader).ToString(), (EventStatuses) teamLeadLastResponse},
                        {((int) EmployeeRoles.Client).ToString(), (EventStatuses) clientResponse},
                        {((int) EmployeeRoles.Cse).ToString(), (EventStatuses) cseResponse},
                        {((int) EmployeeRoles.SystemAdministrator).ToString(), (EventStatuses) adminResponse}
                    };
                default:
                    return new Dictionary<string, EventStatuses>();
            }
        }
        
        private ICollection<EventWorkflowResponder> CreateEventWorkflowResponders(int eventTypeId, EventWorkflow eventWorkflow)
        {          
            var workflowRespondersIdList = new List<int>();
            switch (eventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    workflowRespondersIdList = new List<int>{(int)EmployeeRoles.TeamLeader, (int)EmployeeRoles.Client, (int)EmployeeRoles.Cse};
                    break;
                default:
                    workflowRespondersIdList = new List<int>();
                    break;
            }

            var employeeRoleIdList =_dbContext.EmployeeRoleRepository.GetAsQueryable(x => workflowRespondersIdList.Contains(x.EmployeeRoleId))
                .Select(x => new EventWorkflowResponder
            {
                EventWorkflow = eventWorkflow,
                EmployeeRoleId = x.EmployeeRoleId
            }).ToList();
            
            return employeeRoleIdList;
        }

        private int GetInitialWorkflowState(int eventTypeId)
        {
            switch (eventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    return (int) PtoState.LeaveAwaitingTeamLeadClient;
                default:
                    return 0;
            }
        }
    }
}