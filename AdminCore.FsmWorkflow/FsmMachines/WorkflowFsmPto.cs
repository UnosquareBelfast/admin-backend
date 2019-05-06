using System;
using System.Collections.Generic;
using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using Stateless;
using Stateless.Reflection;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public class WorkflowFsmPto : WorkflowFsm<WorkflowStateData, PtoState, LeaveTriggersPto>
    {       
        private StateMachine<PtoState, LeaveTriggersPto>.TriggerWithParameters<EventStatuses, string> LeaveResponseTrigger;

        private EventStatuses _currentEventStatus = EventStatuses.AwaitingApproval;
        private string _message = EventStatuses.AwaitingApproval.ToString();
        
        public WorkflowFsmPto(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            
            ConfigureFsm();
        }
        
        protected override void ConfigureFsm()
        {
            FsMachine = new StateMachine<PtoState, LeaveTriggersPto>(() => (PtoState)FsmStateData.CurrentState, s => FsmStateData.CurrentState = (int)s);
            
            LeaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersPto.LeaveResponded);

//            FsMachine.OnUnhandledTrigger((states, events) => { });
            
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
                .InternalTransition(LeaveResponseTrigger,
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
                    switch (FsmStateData.ApprovalDict[FsmStateData.Cse])
                    {
                        case EventStatuses.Approved:
                            FsMachine.Fire(LeaveTriggersPto.LeaveApproved);
                            break;
                        case EventStatuses.Rejected:
                            FsMachine.Fire(LeaveTriggersPto.LeaveRejected);
                            break;
                    }
                })
                .InternalTransition(LeaveResponseTrigger,
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
            _currentEventStatus = EventStatuses.Approved;
            _message = "Leave Approved";
        }

        private void LeaveRejected()
        {
            _currentEventStatus = EventStatuses.Rejected;
            _message = "Leave Rejected";
        }

        private void LeaveCancelled()
        {
            _currentEventStatus = EventStatuses.Cancelled;
            _message = "Leave Cancelled";
        }
        
        public override WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder)
        {
            // Fire the response trigger first.
            FsMachine.Fire(LeaveResponseTrigger, approvalState, responder);
            // Then evaluate the changes.
            FsMachine.Fire(LeaveTriggersPto.EvaluateLeaveState);
           
            var machineStateInfo = new WorkflowFsmStateInfo(FsMachine.IsInState(PtoState.LeaveRequestCompleted),
                _currentEventStatus,
                _message);
            
            return machineStateInfo;
        }
    }
}