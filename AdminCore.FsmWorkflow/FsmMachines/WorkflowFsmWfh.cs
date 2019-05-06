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
    public class WorkflowFsmWfh : WorkflowFsm<WorkflowStateData, WfhState, LeaveTriggersWfh>
    {       
        private StateMachine<WfhState, LeaveTriggersWfh>.TriggerWithParameters<EventStatuses, string> LeaveResponseTrigger;

        private EventStatuses _currentEventStatus = EventStatuses.AwaitingApproval;
        private string _message = EventStatuses.AwaitingApproval.ToString();
        
        public WorkflowFsmWfh(WorkflowStateData fsmStateData)
        {
            FsmStateData = fsmStateData;
            
            ConfigureFsm();
        }
        
        protected override void ConfigureFsm()
        {
            FsMachine = new StateMachine<WfhState, LeaveTriggersWfh>(() => (WfhState)FsmStateData.CurrentState, s => FsmStateData.CurrentState = (int)s);
            
            LeaveResponseTrigger = FsMachine.SetTriggerParameters<EventStatuses, string>(LeaveTriggersWfh.LeaveResponded);
            
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
                .InternalTransition(LeaveResponseTrigger,
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
        
        private void LeaveResponse(EventStatuses approvalState, string responder)
        {
            if(FsmStateData.ApprovalDict.TryGetValue(responder, out _))
            {
                FsmStateData.ApprovalDict[responder] = approvalState;
            }
        }
        
        private bool IsTeamLeadResponsesReceived(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.TeamLead] != EventStatuses.AwaitingApproval;
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
            FsMachine.Fire(LeaveTriggersWfh.EvaluateLeaveState);
           
            var machineStateInfo = new WorkflowFsmStateInfo(FsMachine.IsInState(WfhState.LeaveRequestCompleted),
                _currentEventStatus,
                _message);
            
            return machineStateInfo;
        }
    }
}