namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers
{
    public enum LeaveTriggersPto
    {
        LeaveApproved,
        LeaveRejected,
        LeaveResponded,
        LeaveCancelled,
        EvaluateLeaveState,
        TeamLeadClientResponseReceived,
        AdminApprove,
        AdminReject
    }
}