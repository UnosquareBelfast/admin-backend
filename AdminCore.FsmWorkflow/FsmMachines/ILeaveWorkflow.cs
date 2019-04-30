using AdminCore.FsmWorkflow.EnumConstants;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public interface ILeaveWorkflow<State>
    {
        State CurrentState { get; }
        
        void FireLeaveResponded(ApprovalState approvalState, string responder);

        string ToJson();
        
        void FromJson(string jsonString);
    }
}