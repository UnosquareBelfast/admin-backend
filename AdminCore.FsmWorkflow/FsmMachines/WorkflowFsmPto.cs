using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmPto : WorkflowFsm<PtoState, LeaveTriggersPto>
    {
        public WorkflowFsmPto(WorkflowStateData fsmStateData) : base(fsmStateData)
        {
            TriggerStateAccessor = new TriggerStateAccessorPto();
        }

        public override void ConfigureFsm(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            FsMachine = new StateMachine<PtoState, LeaveTriggersPto>(() => (PtoState)FsmStateData.CurrentState, currentState => FsmStateData.CurrentState = (int)currentState);

            LeaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersPto.LeaveResponded);

            // Leave Awaiting Team Lead and Client
            ConfigureStateLeaveAwaitingTeamLeadClient(FsMachine);
            ConfigureStateLeaveAwaitingCse(FsMachine);
            ConfigureStateLeaveApproved(FsMachine);
            ConfigureStateLeaveRejected(FsMachine);
            ConfigureStateLeaveCancelled(FsMachine);

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

        private void ConfigureStateLeaveAwaitingTeamLeadClient(StateMachine<PtoState, LeaveTriggersPto> fsMachine)
        {
            fsMachine.Configure(PtoState.LeaveAwaitingTeamLeadClient)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        fsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersPto.AdminApprove
                            : LeaveTriggersPto.AdminReject);
                    }
                    else if (IsTeamLeadClientResponsesReceived(FsmStateData.ApprovalDict))
                    {
                        fsMachine.Fire(LeaveTriggersPto.TeamLeadClientResponseReceived);
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersPto.TeamLeadClientResponseReceived, PtoState.LeaveAwaitingCse)
                .Permit(LeaveTriggersPto.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTriggersPto.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.AdminReject, PtoState.LeaveRejected)
                .PermitReentry(LeaveTriggersPto.EvaluateLeaveState);
        }

        private void ConfigureStateLeaveAwaitingCse(StateMachine<PtoState, LeaveTriggersPto> fsMachine)
        {
            fsMachine.Configure(PtoState.LeaveAwaitingCse)
                .SubstateOf(PtoState.LeaveAwaitingResponses)
                .OnActivate(() =>
                {
                    if (IsAdminResponseReceived(FsmStateData.ApprovalDict))
                    {
                        fsMachine.Fire(IsAdminResponseApprove(FsmStateData.ApprovalDict)
                            ? LeaveTriggersPto.AdminApprove
                            : LeaveTriggersPto.AdminReject);
                    }

                    FireApproveRejectBasedOnResponderResponse(FsmStateData.Cse);
                })
                .InternalTransition(LeaveResponseTrigger,
                    (approvalState, responder, transition) => LeaveResponse(approvalState, responder))
                .Permit(LeaveTriggersPto.LeaveApproved, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.LeaveRejected, PtoState.LeaveRejected)
                .Permit(LeaveTriggersPto.LeaveCancelled, PtoState.LeaveCancelled)
                .Permit(LeaveTriggersPto.AdminApprove, PtoState.LeaveApproved)
                .Permit(LeaveTriggersPto.AdminReject, PtoState.LeaveRejected)
                .PermitReentry(LeaveTriggersPto.EvaluateLeaveState);
        }

        private void ConfigureStateLeaveApproved(StateMachine<PtoState, LeaveTriggersPto> fsMachine)
        {
            fsMachine.Configure(PtoState.LeaveApproved)
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
        }

        private void ConfigureStateLeaveRejected(StateMachine<PtoState, LeaveTriggersPto> fsMachine)
        {
            fsMachine.Configure(PtoState.LeaveRejected)
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
        }

        private void ConfigureStateLeaveCancelled(StateMachine<PtoState, LeaveTriggersPto> fsMachine)
        {
            fsMachine.Configure(PtoState.LeaveCancelled)
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
        }
    }
}
