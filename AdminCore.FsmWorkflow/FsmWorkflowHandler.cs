using System.Net.WebSockets;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.FsmWorkflow.EnumConstants;
using AdminCore.FsmWorkflow.FsmMachines;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;

namespace AdminCore.FsmWorkflow
{
    public class FsmWorkflowHandler : IFsmWorkflowHandler
    {
        public FsmWorkflowHandler()
        {
            
        }
       
        public EventWorkflow CreateEventWorkflow(EventTypes eventType, string teamLead, string client, string cse)
        {
            var workflow = new EventWorkflow
            {
                EventId = (int)eventType,
                WorkflowSerializedState = GetSerializedWorkflow(eventType, teamLead, client, cse)
            };

            return workflow;
        }
        
        private string GetSerializedWorkflow(EventTypes eventType, string teamLead, string client, string cse)
        {
            switch (eventType)
            {
                case EventTypes.AnnualLeave:
                    return new WorkflowFsmPto(teamLead, client, cse, PtoState.LeaveAwaitingTeamLeadClient).ToJson();
                default:
                    return "";
            }
        }

        private void FireLeaveResponded(EventTypes eventType, string serializedFsm, ApprovalState approvalState, string responderName)
        {
            switch (eventType)
            {
                case EventTypes.AnnualLeave:
                    var workflow = new WorkflowFsmPto();
                    workflow.FromJson(serializedFsm);
                    workflow.FireLeaveResponded(approvalState, responderName);
                    break;
                default:
                    return;
            }
        }
    }
}