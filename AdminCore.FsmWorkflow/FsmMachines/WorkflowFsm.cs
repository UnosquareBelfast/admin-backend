using System.Collections.Generic;
using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
using AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor;
using Newtonsoft.Json;
using Stateless;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public abstract class WorkflowFsm<TState, TTrigger> : ILeaveWorkflow
    {
        protected WorkflowStateData FsmStateData { get; set; }

        protected StateMachine<TState, TTrigger> FsMachine;

        protected EventStatuses CurrentEventStatus = EventStatuses.AwaitingApproval;
        protected string Message = EventStatuses.AwaitingApproval.ToString();

        protected ITriggerStateAccessor<TState, TTrigger> TriggerStateAccessor { get; set; }
        protected StateMachine<TState, TTrigger>.TriggerWithParameters<EventStatuses, string> LeaveResponseTrigger;

        public WorkflowFsm(WorkflowStateData fsmStateData)
        {
            ConfigureFsm(fsmStateData);
        }

        public virtual WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder)
        {
            if (approvalState != EventStatuses.Cancelled)
            {
                // Fire the response trigger first.
                FsMachine.Fire(LeaveResponseTrigger, approvalState, responder);
                // Then evaluate the changes.
                FsMachine.Fire(TriggerStateAccessor.EvaluateLeaveState);
            }
            else
            {
                FsMachine.Fire(TriggerStateAccessor.LeaveCancelled);
            }

            var machineStateInfo = new WorkflowFsmStateInfo(FsMachine.IsInState(TriggerStateAccessor.LeaveRequestCompleted),
                CurrentEventStatus,
                Message);

            return machineStateInfo;
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(FsmStateData);
        }

        public virtual void FromJson(string jsonString)
        {
            FsmStateData = JsonConvert.DeserializeObject<WorkflowStateData>(jsonString);
            ConfigureFsm(FsmStateData);
        }

        public abstract void ConfigureFsm(WorkflowStateData fsmStateData);

        protected void LeaveResponse(EventStatuses approvalState, string responder)
        {
            if(FsmStateData.ApprovalDict.TryGetValue(responder, out _))
            {
                FsmStateData.ApprovalDict[responder] = approvalState;
            }
        }

        protected bool IsAdminResponseReceived(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.Admin] != EventStatuses.AwaitingApproval;
        }

        protected bool IsAdminResponseApprove(Dictionary<string, EventStatuses> approvalDict)
        {
            return approvalDict[FsmStateData.Admin] == EventStatuses.Approved;
        }

        protected void LeaveApproved()
        {
            CurrentEventStatus = EventStatuses.Approved;
            Message = "Leave Approved";
        }

        protected void LeaveRejected()
        {
            CurrentEventStatus = EventStatuses.Rejected;
            Message = "Leave Rejected";
        }

        protected void LeaveCancelled()
        {
            CurrentEventStatus = EventStatuses.Cancelled;
            Message = "Leave Cancelled";
        }
    }
}
