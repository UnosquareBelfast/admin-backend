namespace AdminCore.FsmWorkflow.FsmMachines.FsmLeaveStates
{
    public enum PtoState
    {
        LeaveAwaitingResponses,
        LeaveResponsesReceived,
        LeaveAwaitingTeamLeadClient,
        LeaveAwaitingCse,
        LeaveApproved,
        LeaveRejected
    }
}