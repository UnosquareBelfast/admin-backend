using System.Net.WebSockets;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
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
    }
}