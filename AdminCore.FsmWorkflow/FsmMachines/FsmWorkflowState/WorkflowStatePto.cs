using System.Collections.Generic;
using AdminCore.FsmWorkflow.EnumConstants;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using Newtonsoft.Json;

namespace AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState
{
    public class WorkflowStatePto
    {
        public PtoState CurrentState { get; set; }
        
        public Dictionary<string, ApprovalState> ApprovalDict { get; set; }

        [JsonConstructor]
        public WorkflowStatePto(PtoState currentState, Dictionary<string, ApprovalState> approvalDict)
        {
            CurrentState = currentState;

            ApprovalDict = approvalDict;
        } 
        
        public WorkflowStatePto(string teamLead, string client, string cse, PtoState initialState)
        {
            CurrentState = initialState;
            
            ApprovalDict = new Dictionary<string, ApprovalState>
            {
                {teamLead, ApprovalState.Unassigned},
                {client, ApprovalState.Unassigned},
                {cse, ApprovalState.Unassigned}
            };
        }
    }
}