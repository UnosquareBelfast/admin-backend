using System.Collections.Generic;
using System.Linq;
using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow.FsmMachines;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Microsoft.EntityFrameworkCore;

namespace AdminCore.FsmWorkflow
{
    public class FsmWorkflowHandler : IFsmWorkflowHandler
    {
        private IDatabaseContext _dbContext;
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
            
            _dbContext.EventWorkflowRepository.Insert(eventWorkflow);
            _dbContext.SaveChanges();
            
            return eventWorkflow;
        }

        public WorkflowFsmStateInfo FireLeaveResponse(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatus, EventWorkflow eventWorkflow)
        {
            WorkflowFsmStateInfo workflowFsmStateInfo = null;
            var workflowStateData = RebuildWorkflowStateData(eventWorkflow);
            switch (employeeEvent.EventTypeId)
            {
                case (int)EventTypes.AnnualLeave:
                    var workflowFsm = new WorkflowFsmPto(workflowStateData);
                    
                    // Cast is not redundant, need the int value of enum as a string
                    // ReSharper disable once RedundantCast
                    workflowFsmStateInfo = workflowFsm.FireLeaveResponded(eventStatus, ((int)respondeeEmployee.EmployeeRoleId).ToString());
                    eventWorkflow = RecreateEventWorkflowForSave(employeeEvent, respondeeEmployee, eventWorkflow, workflowStateData, workflowFsmStateInfo.CurrentEventStatuses);
                    break;
            }

            _dbContext.EventWorkflowRepository.Update(eventWorkflow);
            _dbContext.SaveChanges();
            
            return workflowFsmStateInfo;
        }

        private WorkflowStateData RebuildWorkflowStateData(EventWorkflow eventWorkflow)
        {
            var approvalDict = RebuildApprovalDict(eventWorkflow);
                    
            return new WorkflowStateData(eventWorkflow.WorkflowState,
                ((int)EmployeeRoles.TeamLeader).ToString(),
                ((int)EmployeeRoles.Client).ToString(),
                ((int)EmployeeRoles.Cse).ToString(),
                ((int)EmployeeRoles.SystemAdministrator).ToString(),
                approvalDict);
        }
        
        private Dictionary<string, EventStatuses> RebuildApprovalDict(EventWorkflow eventWorkflow)
        {
            var teamLeadLastResponse = eventWorkflow.EventWorkflowApprovalResponses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.TeamLeader)?.EventStatusId
                                       ?? (int)EventStatuses.AwaitingApproval;
            var clientResponse = eventWorkflow.EventWorkflowApprovalResponses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Client)?.EventStatusId
                                     ?? (int)EventStatuses.AwaitingApproval;
            var cseResponse = eventWorkflow.EventWorkflowApprovalResponses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.Cse)?.EventStatusId 
                                  ?? (int)EventStatuses.AwaitingApproval;
            var adminResponse = eventWorkflow.EventWorkflowApprovalResponses.LastOrDefault(x => x.EmployeeRoleId == (int)EmployeeRoles.SystemAdministrator)?.EventStatusId 
                                  ?? (int)EventStatuses.AwaitingApproval;

            return new Dictionary<string, EventStatuses>
            {
                {((int) EmployeeRoles.TeamLeader).ToString(), (EventStatuses) teamLeadLastResponse},
                {((int) EmployeeRoles.Client).ToString(), (EventStatuses) clientResponse},
                {((int) EmployeeRoles.Cse).ToString(), (EventStatuses) cseResponse},
                {((int) EmployeeRoles.SystemAdministrator).ToString(), (EventStatuses) adminResponse}
            };
        }

        private EventWorkflow RecreateEventWorkflowForSave(EventDto employeeEvent, EmployeeDto respondeeEmployee, 
            EventWorkflow eventWorkflow, WorkflowStateData workflowStateData, EventStatuses currentEventStatus)
        {
            eventWorkflow.WorkflowState = workflowStateData.CurrentState;
            
            eventWorkflow.EventWorkflowApprovalResponses.Add(new EmployeeApprovalResponse
            {
                EmployeeId = respondeeEmployee.EmployeeId,
                EmployeeRoleId = respondeeEmployee.EmployeeRoleId,
                EventStatusId = (int)currentEventStatus,
                EventWorkflowId = eventWorkflow.EventWorkflowId
            });
            
            return eventWorkflow;
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