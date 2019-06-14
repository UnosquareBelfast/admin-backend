using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;

namespace AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor
{
    public class TriggerStateAccessorWfh : ITriggerStateAccessor<WfhState, LeaveTriggersWfh>
    {
        public LeaveTriggersWfh LeaveCancelled => LeaveTriggersWfh.LeaveCancelled;
        public LeaveTriggersWfh EvaluateLeaveState => LeaveTriggersWfh.EvaluateLeaveState;

        public WfhState LeaveAwaitingResponses => WfhState.LeaveAwaitingResponses;
        public WfhState LeaveRequestCompleted => WfhState.LeaveRequestCompleted;
    }
}
