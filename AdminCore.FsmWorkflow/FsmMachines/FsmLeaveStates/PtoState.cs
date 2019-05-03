namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates
{
    public enum PtoState
    {
        LeaveAwaitingResponses,
            // Substate
            LeaveAwaitingTeamLeadClient,
            LeaveAwaitingCse,
        LeaveRequestCompleted,
            // Substate
            LeaveApproved,
            LeaveRejected,
            LeaveCancelled
    }
}