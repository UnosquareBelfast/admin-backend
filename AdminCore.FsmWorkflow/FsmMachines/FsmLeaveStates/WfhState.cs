namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates
{
    public enum WfhState
    {
        LeaveAwaitingResponses,
            // Substate
            LeaveAwaitingTeamLead,
        LeaveRequestCompleted,
            // Substate
            LeaveApproved,
            LeaveRejected,
            LeaveCancelled
    }
}