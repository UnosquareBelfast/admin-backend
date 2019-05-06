using AdminCore.Common;
using AdminCore.Constants.Enums;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public interface ILeaveWorkflow
    {
        WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder);

        string ToJson();
        
        void FromJson(string jsonString);
    }
}