using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using Newtonsoft.Json;

namespace AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState
{
    public class WorkflowStatePto
    {
        public PtoState CurrentState { get; set; }
        
        public string TeamLead { get; set; }
        public string Client { get; set; }
        public string Cse { get; set; }
        public string Admin { get; set; }
        
        public Dictionary<string, EventStatuses> ApprovalDict { get; set; }

        [JsonConstructor]
        public WorkflowStatePto(PtoState currentState, Dictionary<string, EventStatuses> approvalDict)
        {
            CurrentState = currentState;

            ApprovalDict = approvalDict;
        } 
        
        public WorkflowStatePto(string teamLead, string client, string cse, string admin, PtoState initialState)
        {
            CurrentState = initialState;

            TeamLead = teamLead;
            Client = client;
            Cse = cse;
            Admin = admin;
            
            ApprovalDict = new Dictionary<string, EventStatuses>
            {
                {teamLead, EventStatuses.AwaitingApproval},
                {client, EventStatuses.AwaitingApproval},
                {cse, EventStatuses.AwaitingApproval},
                {admin, EventStatuses.AwaitingApproval}
            };
        }
    }
}