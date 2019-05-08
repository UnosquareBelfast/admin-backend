using AdminCore.Common;
using AdminCore.Constants.Enums;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;

namespace AdminCore.FsmWorkflow.FsmMachines
{
    public interface ILeaveWorkflow
    {
        WorkflowFsmStateInfo FireLeaveResponded(EventStatuses approvalState, string responder);

        void ConfigureFsm(WorkflowStateData fsmStateData);
        
        string ToJson();
        
        void FromJson(string jsonString);
    }
}