using System.Collections.Generic;
using System.Linq;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow.EnumConstants;
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
            var workflow = new EventWorkflow
            {
                EventId = eventId,
                EmployeeRoleResponders = CreateListRoleResponders(eventTypeId),
                WorkflowState = GetInitialWorkflowState(eventTypeId)
            };

            return workflow;
        }

        public bool FireLeaveResponse(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatus, EventWorkflow eventWorkflow)
        {
            var approvalState = ApprovalState.Unassigned;
            switch (eventStatus)
            {
                case EventStatuses.Approved:
                    approvalState = ApprovalState.Approved;
                    break;
                case EventStatuses.Rejected:
                    approvalState = ApprovalState.Rejected;
                    break;
                case EventStatuses.Cancelled:
                    approvalState = ApprovalState.Cancelled;
                    break;
            }
            
            switch (employeeEvent.EventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    var approvalDict = RebuildApprovalDictionary(employeeEvent.EventTypeId, eventWorkflow);
                    
                    var workflowStateData = new WorkflowStatePto((PtoState)eventWorkflow.WorkflowState, approvalDict);
                    var workflowFsm = new WorkflowFsmPto(workflowStateData);
                    
                    // Cast is not redundant, need the int value of enum as a string
                    // ReSharper disable once RedundantCast
                    return workflowFsm.FireLeaveResponded(approvalState, ((int)respondeeEmployee.EmployeeRoleId).ToString());
                default:
                    return false;
            }
        }

        private Dictionary<string, ApprovalState> RebuildApprovalDictionary(int eventTypeId, EventWorkflow eventWorkflow)
        {
            switch (eventTypeId)
            {
                case (int) EventTypes.AnnualLeave:
                    var teamLeadLastResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.TeamLeader)?.ApprovalStatusId
                                               ?? (int)ApprovalState.Unassigned;
                    var clientResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Client)?.ApprovalStatusId
                                             ?? (int)ApprovalState.Unassigned;
                    var cseResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Cse)?.ApprovalStatusId 
                                          ?? (int)ApprovalState.Unassigned;
                    var adminResponse = eventWorkflow.EventWorkflowApprovalStatuses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.SystemAdministrator)?.ApprovalStatusId 
                                          ?? (int)ApprovalState.Unassigned;

                    return new Dictionary<string, ApprovalState>
                    {
                        {((int) EmployeeRoles.TeamLeader).ToString(), (ApprovalState) teamLeadLastResponse},
                        {((int) EmployeeRoles.Client).ToString(), (ApprovalState) clientResponse},
                        {((int) EmployeeRoles.Cse).ToString(), (ApprovalState) cseResponse},
                        {((int) EmployeeRoles.SystemAdministrator).ToString(), (ApprovalState) adminResponse}
                    };
                default:
                    return new Dictionary<string, ApprovalState>();
            }
        }
        
        private ICollection<EmployeeRole> CreateListRoleResponders(int eventTypeId)
        {          
            var employeeRoleIdList = new List<int>();
            switch (eventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    employeeRoleIdList = new List<int>{(int)EmployeeRoles.TeamLeader, (int)EmployeeRoles.Client, (int)EmployeeRoles.Cse};
                    break;
                default:
                    employeeRoleIdList = new List<int>();
                    break;
            }
            
            var employeeRoleList = _dbContext.EmployeeRoleRepository.GetAsQueryable(x => employeeRoleIdList.Contains(x.EmployeeRoleId)).ToList();
            return employeeRoleList;
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