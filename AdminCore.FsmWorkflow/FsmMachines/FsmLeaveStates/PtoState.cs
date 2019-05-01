namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates
{
    public enum PtoState
    {
        LeaveAwaitingResponses,
        LeaveRequestCompleted,
        LeaveAwaitingTeamLeadClient,
        LeaveAwaitingCse,
        LeaveApproved,
        LeaveRejected,
        LeaveCancelled
    }
}