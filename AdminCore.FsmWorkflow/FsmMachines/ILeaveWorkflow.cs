using AdminCore.Constants.Enums;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public interface ILeaveWorkflow<State>
    {
        State CurrentState { get; }
        
        bool FireLeaveResponded(EventStatuses approvalState, string responder);

        string ToJson();
        
        void FromJson(string jsonString);
    }
}