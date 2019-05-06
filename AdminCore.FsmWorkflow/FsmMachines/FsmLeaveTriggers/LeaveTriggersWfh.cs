namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveTriggers
{
    public enum LeaveTriggersWfh
    {
        LeaveApproved,
        LeaveRejected,
        LeaveResponded,
        LeaveCancelled,
        EvaluateLeaveState,
        TeamLeadResponseReceived,
        AdminApprove,
        AdminReject
    }
}