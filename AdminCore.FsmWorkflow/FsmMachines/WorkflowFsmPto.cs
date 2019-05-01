using System;
using System.Collections.Generic;
using AdminCore.FsmWorkflow.EnumConstants;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmPto : WorkflowFsm<WorkflowStatePto, PtoState, LeaveTrigger>
    {       
        private StateMachine<PtoState, LeaveTrigger>.TriggerWithParameters<ApprovalState, string> LeaveResponseTrigger;
        
//        private string _teamLead, _client, _cse;

        public WorkflowFsmPto()
        {
            
        }
        
        public WorkflowFsmPto(string teamLead, string client, string cse, PtoState initialState)
        {           
            FsmStateData = new WorkflowStatePto(teamLead, client, cse, initialState);

            ConfigureFsm();
        }

        public WorkflowFsmPto(WorkflowStatePto fsmStateData)
        {
            FsmStateData = fsmStateData;

            ConfigureFsm();
        }
        
        protected override void ConfigureFsm()
        {
//            _teamLead = FsmStateData.TeamLead;
//            _client = FsmStateData.Client;
//            _cse = FsmStateData.Cse;
            
            FsMachine = new StateMachine<PtoState, LeaveTrigger>(() => FsmStateData.CurrentState, s => FsmStateData.CurrentState = s);
            
            LeaveResponseTrigger = FsMachine.SetTriggerParameters<ApprovalState, string>(LeaveTrigger.LeaveResponded);

            // Leave Awaiting Team Lead and Client
            FsMachine.Configure(PtoState.LeaveAwaitingTeamLeadClient)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsTeamLeadClientResponsesReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(LeaveTrigger.TeamLeadClientResponseReceived);
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTrigger.TeamLeadClientResponseReceived, PtoState.LeaveAwaitingCse)
                .Permit(LeaveTrigger.LeaveCancelled, PtoState.LeaveCancelled)
                .PermitReentry(LeaveTrigger.EvaluateLeaveState);
            
            // Leave Awaiting CSE
            FsMachine.Configure(PtoState.LeaveAwaitingCse)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    switch (FsmStateData.ApprovalDict[FsmStateData.Cse])
                    {
                        case ApprovalState.Approved:
                            FsMachine.Fire(LeaveTrigger.LeaveApproved);
                            break;
                        case ApprovalState.Rejected:
                            FsMachine.Fire(LeaveTrigger.LeaveRejected);
                            break;
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTrigger.LeaveApproved, PtoState.LeaveApproved)
                .Permit(LeaveTrigger.LeaveRejected, PtoState.LeaveRejected)
                .Permit(LeaveTrigger.LeaveCancelled, PtoState.LeaveCancelled)
                .PermitReentry(LeaveTrigger.EvaluateLeaveState);
            
            // Leave Approved
            FsMachine.Configure(PtoState.LeaveApproved)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveApproved);
            
            // Leave Rejected
            FsMachine.Configure(PtoState.LeaveRejected)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveRejected);
            
            // Leave Cancelled
            FsMachine.Configure(PtoState.LeaveCancelled)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveCancelled);
            
            FsMachine.Activate();
        }
        
        private void LeaveResponse(ApprovalState approvalState, string responder)
        {
            if(FsmStateData.ApprovalDict.TryGetValue(responder, out _))
            {
                FsmStateData.ApprovalDict[responder] = approvalState;
            }
        }
        
        private bool IsTeamLeadClientResponsesReceived(Dictionary<string, ApprovalState> approvalDict)
        {
            return approvalDict[FsmStateData.TeamLead] != ApprovalState.Unassigned &&
                   approvalDict[FsmStateData.Client] != ApprovalState.Unassigned;
        }

        private void LeaveApproved()
        {
            Console.WriteLine("LEAVE APPROVED");
        }

        private void LeaveRejected()
        {
            Console.WriteLine("LEAVE REJECTED");
        }

        private void LeaveCancelled()
        {
            Console.WriteLine("LEAVE CANCELLED");
        }
        
        public override bool FireLeaveResponded(ApprovalState approvalState, string responder)
        {
            // Fire the response trigger first.
            FsMachine.Fire(LeaveResponseTrigger, approvalState, responder);
            // Then evaluate the changes.
            FsMachine.Fire(LeaveTrigger.EvaluateLeaveState);

            return FsMachine.IsInState(PtoState.LeaveRequestCompleted);
        }
    }
}