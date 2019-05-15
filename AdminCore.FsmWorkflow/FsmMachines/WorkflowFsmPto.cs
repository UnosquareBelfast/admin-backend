using System.Collections.Generic;
using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmPto : WorkflowFsm<PtoState, LeaveTriggersPto>
    {
        private StateMachine<PtoState, LeaveTriggersPto>.TriggerWithParameters<EventStatuses, string> _leaveResponseTrigger;

        public WorkflowFsmPto(WorkflowStateData fsmStateData)
        {
            ConfigureFsm(fsmStateData);
        }

        public override void ConfigureFsm(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            FsMachine = new StateMachine<PtoState, LeaveTriggersPto>(() => (PtoState)FsmStateData.CurrentState, currentState => FsmStateData.CurrentState = (int)currentState);

            _leaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersPto.LeaveResponded);

            // Leave Awaiting Team Lead and Client
            FsMachine.Configure(PtoState.LeaveAwaitingTeamLeadClient)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersPto.AdminApprove
                            : LeaveTriggersPto.AdminReject);
                    }
                    else if (IsTeamLeadClientResponsesReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(LeaveTriggersPto.TeamLeadClientResponseReceived);
                    }
                })
                .InternalTransition(_leaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersPto.TeamLeadClientResponseReceived, PtoState.LeaveAwaitingCse)
                .Permit(LeaveTriggersPto.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTriggersPto.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.AdminReject, PtoState.LeaveRejected)
                .PermitReentry(LeaveTriggersPto.EvaluateLeaveState);

            // Leave Awaiting CSE
            FsMachine.Configure(PtoState.LeaveAwaitingCse)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersPto.AdminApprove
                            : LeaveTriggersPto.AdminReject);
                    }

                    FireApproveRejectBasedOnResponderResponse(FsmStateData.Cse);
                })
                .InternalTransition(_leaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersPto.LeaveApproved, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.LeaveRejected, PtoState.LeaveRejected)
                .Permit(LeaveTriggersPto.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTriggersPto.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.AdminReject, PtoState.LeaveRejected)
                .PermitReentry(LeaveTriggersPto.EvaluateLeaveState);

            // Leave Approved
            FsMachine.Configure(PtoState.LeaveApproved)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveApproved)
                .Ignore(LeaveTriggersPto.AdminReject)
                .Ignore(LeaveTriggersPto.AdminApprove)
                .Ignore(LeaveTriggersPto.LeaveApproved)
                .Ignore(LeaveTriggersPto.LeaveRejected)
                .Ignore(LeaveTriggersPto.LeaveCancelled)
                .Ignore(LeaveTriggersPto.LeaveResponded)
                .Ignore(LeaveTriggersPto.EvaluateLeaveState)
                .Ignore(LeaveTriggersPto.TeamLeadClientResponseReceived);

            // Leave Rejected
            FsMachine.Configure(PtoState.LeaveRejected)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveRejected)
                .Ignore(LeaveTriggersPto.AdminReject)
                .Ignore(LeaveTriggersPto.AdminApprove)
                .Ignore(LeaveTriggersPto.LeaveApproved)
                .Ignore(LeaveTriggersPto.LeaveRejected)
                .Ignore(LeaveTriggersPto.LeaveCancelled)
                .Ignore(LeaveTriggersPto.LeaveResponded)
                .Ignore(LeaveTriggersPto.EvaluateLeaveState)
                .Ignore(LeaveTriggersPto.TeamLeadClientResponseReceived);

            // Leave Cancelled
            FsMachine.Configure(PtoState.LeaveCancelled)
                .SubstateOf(PtoState.LeaveRequestCompleted)
                .OnEntry(LeaveCancelled)
                .Ignore(LeaveTriggersPto.AdminReject)
                .Ignore(LeaveTriggersPto.AdminApprove)
                .Ignore(LeaveTriggersPto.LeaveApproved)
                .Ignore(LeaveTriggersPto.LeaveRejected)
                .Ignore(LeaveTriggersPto.LeaveCancelled)
                .Ignore(LeaveTriggersPto.LeaveResponded)
                .Ignore(LeaveTriggersPto.EvaluateLeaveState)
                .Ignore(LeaveTriggersPto.TeamLeadClientResponseReceived);

            FsMachine.Activate();
        }

        private bool IsTeamLeadClientResponsesReceived(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.TeamLead] != EventStatuses.AwaitingApproval &&
                   approvalDict[FsmStateData.Client] != EventStatuses.AwaitingApproval;
        }

        private void FireApproveRejectBasedOnResponderResponse(string responder)
        {
            switch (FsmStateData.ApprovalDict[responder])
            {
                case EventStatuses.Approved:
                    FsMachine.Fire(LeaveTriggersPto.LeaveApproved);
                    break;
                case EventStatuses.Rejected:
                    FsMachine.Fire(LeaveTriggersPto.LeaveRejected);
                    break;
            }
        }

        public override WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder)
        {
            if (approvalState != EventStatuses.Cancelled)
            {
                // Fire the response trigger first.
                FsMachine.Fire(_leaveResponseTrigger, approvalState, responder);
                // Then evaluate the changes.
                FsMachine.Fire(LeaveTriggersPto.EvaluateLeaveState);
            }
            else
            {
                FsMachine.Fire(LeaveTriggersPto.LeaveCancelled);
            }

            var machineStateInfo = new WorkflowFsmStateInfo(FsMachine.IsInState(PtoState.LeaveRequestCompleted),
                CurrentEventStatus,
                Message);

            return machineStateInfo;
        }
    }
}
