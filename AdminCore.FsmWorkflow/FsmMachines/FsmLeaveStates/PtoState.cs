namespace FsmTestApp.FsmMachines.FsmLeaveStates
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