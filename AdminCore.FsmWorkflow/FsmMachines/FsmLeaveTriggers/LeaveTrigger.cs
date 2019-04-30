namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers
{
    public enum LeaveTrigger
    {
        LeaveApproved,
        LeaveRejected,
        LeaveResponded,
        EvaluateLeaveState,
        TeamLeadClientResponseReceived
    }
}