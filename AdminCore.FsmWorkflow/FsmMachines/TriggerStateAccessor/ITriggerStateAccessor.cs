namespace AdminCore.FsmWorkflow.FsmMachines.TriggerStateAccessor
{
    public interface ITriggerStateAccessor<TState, TTrigger>
    {
        TTrigger LeaveCancelled { get; }
        TTrigger EvaluateLeaveState { get; }
        TState LeaveAwaitingResponses { get; }
        TState LeaveRequestCompleted { get; }
    }
}
