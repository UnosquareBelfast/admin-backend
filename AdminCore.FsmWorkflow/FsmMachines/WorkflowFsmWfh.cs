using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmWfh : WorkflowFsm<WfhState, LeaveTriggersWfh>
    {
        private StateMachine<WfhState, LeaveTriggersWfh>.TriggerWithParameters<EventStatuses, string> _leaveResponseTrigger;

        public WorkflowFsmWfh(WorkflowStateData fsmStateData)
        {
            ConfigureFsm(fsmStateData);
        }

        public override void ConfigureFsm(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            FsMachine = new StateMachine<WfhState, LeaveTriggersWfh>(() => (WfhState)FsmStateData.CurrentState, currentState => FsmStateData.CurrentState = (int)currentState);

            _leaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersWfh.LeaveResponded);

            // Leave Awaiting Team Lead and Client
            FsMachine.Configure(WfhState.LeaveAwaitingTeamLead)
                .SubstateOf(WfhState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        FsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersWfh.AdminApprove
                            : LeaveTriggersWfh.AdminReject);
                    }
                    else
                    {
                        switch (FsmStateData.ApprovalDict[FsmStateData.TeamLead])
                        {
                            case EventStatuses.Approved:
                                FsMachine.Fire(LeaveTriggersWfh.LeaveApproved);
                                break;
                            case EventStatuses.Rejected:
                                FsMachine.Fire(LeaveTriggersWfh.LeaveRejected);
                                break;
                        }
                    }
                })
                .InternalTransition(_leaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersWfh.LeaveApproved, WfhState.LeaveApproved)
                .Permit(LeaveTriggersWfh.LeaveRejected, WfhState.LeaveRejected)
                .Permit(LeaveTriggersWfh.LeaveCancelled, WfhState.LeaveCancelled)
                .Permit(LeaveTriggersWfh.AdminApprove, WfhState.LeaveApproved)
                .Permit(LeaveTriggersWfh.AdminReject, WfhState.LeaveRejected)
                .PermitReentry(LeaveTriggersWfh.EvaluateLeaveState);

            // Leave Approved
            FsMachine.Configure(WfhState.LeaveApproved)
                .SubstateOf(WfhState.LeaveRequestCompleted)
                .OnEntry(LeaveApproved)
                .Ignore(LeaveTriggersWfh.AdminReject)
                .Ignore(LeaveTriggersWfh.AdminApprove)
                .Ignore(LeaveTriggersWfh.LeaveApproved)
                .Ignore(LeaveTriggersWfh.LeaveRejected)
                .Ignore(LeaveTriggersWfh.LeaveCancelled)
                .Ignore(LeaveTriggersWfh.LeaveResponded)
                .Ignore(LeaveTriggersWfh.EvaluateLeaveState)
                .Ignore(LeaveTriggersWfh.TeamLeadResponseReceived);

            // Leave Rejected
            FsMachine.Configure(WfhState.LeaveRejected)
                .SubstateOf(WfhState.LeaveRequestCompleted)
                .OnEntry(LeaveRejected)
                .Ignore(LeaveTriggersWfh.AdminReject)
                .Ignore(LeaveTriggersWfh.AdminApprove)
                .Ignore(LeaveTriggersWfh.LeaveApproved)
                .Ignore(LeaveTriggersWfh.LeaveRejected)
                .Ignore(LeaveTriggersWfh.LeaveCancelled)
                .Ignore(LeaveTriggersWfh.LeaveResponded)
                .Ignore(LeaveTriggersWfh.EvaluateLeaveState)
                .Ignore(LeaveTriggersWfh.TeamLeadResponseReceived);

            // Leave Cancelled
            FsMachine.Configure(WfhState.LeaveCancelled)
                .SubstateOf(WfhState.LeaveRequestCompleted)
                .OnEntry(LeaveCancelled)
                .Ignore(LeaveTriggersWfh.AdminReject)
                .Ignore(LeaveTriggersWfh.AdminApprove)
                .Ignore(LeaveTriggersWfh.LeaveApproved)
                .Ignore(LeaveTriggersWfh.LeaveRejected)
                .Ignore(LeaveTriggersWfh.LeaveCancelled)
                .Ignore(LeaveTriggersWfh.LeaveResponded)
                .Ignore(LeaveTriggersWfh.EvaluateLeaveState)
                .Ignore(LeaveTriggersWfh.TeamLeadResponseReceived);

            FsMachine.Activate();
        }

        public override WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder)
        {
            if (approvalState != EventStatuses.Cancelled)
            {
                // Fire the response trigger first.
                FsMachine.Fire(_leaveResponseTrigger, approvalState, responder);
                // Then evaluate the changes.
                FsMachine.Fire(LeaveTriggersWfh.EvaluateLeaveState);
            }
            else
            {
                FsMachine.Fire(LeaveTriggersWfh.LeaveCancelled);
            }

            var machineStateInfo = new WorkflowFsmStateInfo(FsMachine.IsInState(WfhState.LeaveRequestCompleted),
                CurrentEventStatus,
                Message);

            return machineStateInfo;
        }
    }
}
