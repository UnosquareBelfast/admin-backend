using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates;
using AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers;

namespace AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor
{
    public class TriggerStateAccessorPto : ITriggerStateAccessor<PtoState, LeaveTriggersPto>
    {
        public LeaveTriggersPto LeaveCancelled => LeaveTriggersPto.LeaveCancelled;
        public LeaveTriggersPto EvaluateLeaveState => LeaveTriggersPto.EvaluateLeaveState;

        public PtoState LeaveAwaitingResponses => PtoState.LeaveAwaitingResponses;
        public PtoState LeaveRequestCompleted => PtoState.LeaveRequestCompleted;
    }
}
