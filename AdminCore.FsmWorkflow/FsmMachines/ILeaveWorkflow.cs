namespace FsmTestApp.FsmMachines
{
    public interface ILeaveWorkflow<State, ApprovalState>
    {
        State CurrentState { get; }
        
        void FireLeaveResponded(ApprovalState approvalState, string responder);

        string ToJson();
        
        void FromJson(string jsonString);
    }
}