using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmWfh : WorkflowFsm<WfhState, LeaveTriggersWfh>
    {
        public WorkflowFsmWfh(WorkflowStateData fsmStateData) : base(fsmStateData)
        {
            TriggerStateAccessor = new TriggerStateAccessorWfh();
        }

        public override void ConfigureFsm(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            FsMachine = new StateMachine<WfhState, LeaveTriggersWfh>(() => (WfhState)FsmStateData.CurrentState, currentState => FsmStateData.CurrentState = (int)currentState);

            LeaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersWfh.LeaveResponded);

            ConfigureStateLeaveAwaitingTeamLead(FsMachine);
            ConfigureStateLeaveApproved(FsMachine);
            ConfigureStateLeaveRejected(FsMachine);
            ConfigureStateLeaveCancelled(FsMachine);

            FsMachine.Activate();
        }

        private void FireApproveRejectBasedOnResponderResponse(string responder)
        {
            switch (FsmStateData.ApprovalDict[responder])
            {
                case EventStatuses.Approved:
                    FsMachine.Fire(LeaveTriggersWfh.LeaveApproved);
                    break;
                case EventStatuses.Rejected:
                    FsMachine.Fire(LeaveTriggersWfh.LeaveRejected);
                    break;
            }
        }

        private void ConfigureStateLeaveAwaitingTeamLead(StateMachine<WfhState, LeaveTriggersWfh> fsMachine)
        {
            fsMachine.Configure(WfhState.LeaveAwaitingTeamLead)
                .SubstateOf(WfhState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        fsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersWfh.AdminApprove
                            : LeaveTriggersWfh.AdminReject);
                    }
                    else
                    {
                        FireApproveRejectBasedOnResponderResponse(FsmStateData.TeamLead);
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersWfh.LeaveApproved, WfhState.LeaveApproved)
                .Permit(LeaveTriggersWfh.LeaveRejected, WfhState.LeaveRejected)
                .Permit(LeaveTriggersWfh.LeaveCancelled, WfhState.LeaveCancelled)
                .Permit(LeaveTriggersWfh.AdminApprove, WfhState.LeaveApproved)
                .Permit(LeaveTriggersWfh.AdminReject, WfhState.LeaveRejected)
                .PermitReentry(LeaveTriggersWfh.EvaluateLeaveState);
        }

        private void ConfigureStateLeaveApproved(StateMachine<WfhState, LeaveTriggersWfh> fsMachine)
        {
            fsMachine.Configure(WfhState.LeaveApproved)
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
        }

        private void ConfigureStateLeaveRejected(StateMachine<WfhState, LeaveTriggersWfh> fsMachine)
        {
            fsMachine.Configure(WfhState.LeaveRejected)
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
        }

        private void ConfigureStateLeaveCancelled(StateMachine<WfhState, LeaveTriggersWfh> fsMachine)
        {
            fsMachine.Configure(WfhState.LeaveCancelled)
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
        }
    }
}
