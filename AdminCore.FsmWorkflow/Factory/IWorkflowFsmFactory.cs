using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;

namespace AdminCore.FsmWorkflow.Factory
{
    public interface IWorkflowFsmFactory<TWorkflowFsm>
    {
        TWorkflowFsm GetWorkflowPto(WorkflowStateData workflowStateData);
        TWorkflowFsm GetWorkflowWfh(WorkflowStateData workflowStateData);
    }
}