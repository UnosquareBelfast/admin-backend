using System.Collections.Generic;
using AdminCore.Constants.Enums;
using Newtonsoft.Json;

namespace AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState
{
    public class WorkflowStateData
    {
        public int CurrentState { get; set; }
        
        public string TeamLead { get; set; }
        public string Client { get; set; }
        public string Cse { get; set; }
        public string Admin { get; set; }
        
        public Dictionary<string, EventStatuses> ApprovalDict { get; set; }

        [JsonConstructor]
        public WorkflowStateData(int currentState, string teamLead, string client, string cse, string admin, Dictionary<string, EventStatuses> approvalDict)
        {
            CurrentState = currentState;

            TeamLead = teamLead;
            Client = client;
            Cse = cse;
            Admin = admin;
            
            ApprovalDict = approvalDict;
        } 
        
        public WorkflowStateData(string teamLead, string client, string cse, string admin, int initialState)
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