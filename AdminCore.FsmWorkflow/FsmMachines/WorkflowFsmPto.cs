using System;
using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmPto : WorkflowFsm<WorkflowStatePto, PtoState, LeaveTrigger>
    {       
        private StateMachine<PtoState, LeaveTrigger>.TriggerWithParameters<EventStatuses, string> LeaveResponseTrigger;


        public WorkflowFsmPto(WorkflowStatePto fsmStateData)
        {
            FsmStateData = fsmStateData;

            ConfigureFsm();
        }
        
        protected override void ConfigureFsm()
        {
            FsMachine = new StateMachine<PtoState, LeaveTrigger>(() => FsmStateData.CurrentState, s => FsmStateData.CurrentState = s);
            
            LeaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTrigger.LeaveResponded);

            // Leave Awaiting Team Lead and Client
            FsMachine.Configure(PtoState.LeaveAwaitingTeamLeadClient)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTrigger.AdminApprove
                            : LeaveTrigger.AdminReject);
                    }
                    else if (IsTeamLeadClientResponsesReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(LeaveTrigger.TeamLeadClientResponseReceived);
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTrigger.TeamLeadClientResponseReceived, PtoState.LeaveAwaitingCse)
                .Permit(LeaveTrigger.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTrigger.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTrigger.AdminReject, PtoState.LeaveRejected)
                .PermitReentry(LeaveTrigger.EvaluateLeaveState);
            
            // Leave Awaiting CSE
            FsMachine.Configure(PtoState.LeaveAwaitingCse)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTrigger.AdminApprove
                            : LeaveTrigger.AdminReject);
                    }
                    switch (FsmStateData.ApprovalDict[FsmStateData.Cse])
                    {
                        case EventStatuses.Approved:
                            FsMachine.Fire(LeaveTrigger.LeaveApproved);
                            break;
                        case EventStatuses.Rejected:
                            FsMachine.Fire(LeaveTrigger.LeaveRejected);
                            break;
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTrigger.LeaveApproved, PtoState.LeaveApproved)
                .Permit(LeaveTrigger.LeaveRejected, PtoState.LeaveRejected)
                .Permit(LeaveTrigger.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTrigger.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTrigger.AdminReject, PtoState.LeaveRejected)
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
        
        private void LeaveResponse(EventStatuses approvalState, string responder)
        {
            if(FsmStateData.ApprovalDict.TryGetValue(responder, out _))
            {
                FsmStateData.ApprovalDict[responder] = approvalState;
            }
        }
        
        private bool IsTeamLeadClientResponsesReceived(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.TeamLead] != EventStatuses.AwaitingApproval &&
                   approvalDict[FsmStateData.Client] != EventStatuses.AwaitingApproval;
        }

        private bool IsAdminResponseReceived(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.Admin] != EventStatuses.AwaitingApproval;
        }
        
        private bool IsAdminResponseApprove(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.Admin] == EventStatuses.Approved;
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
        
        public override bool FireLeaveResponded(EventStatuses approvalState, string responder)
        {
            // Fire the response trigger first.
            FsMachine.Fire(LeaveResponseTrigger, approvalState, responder);
            // Then evaluate the changes.
            FsMachine.Fire(LeaveTrigger.EvaluateLeaveState);

            return FsMachine.IsInState(PtoState.LeaveRequestCompleted);
        }
    }
}