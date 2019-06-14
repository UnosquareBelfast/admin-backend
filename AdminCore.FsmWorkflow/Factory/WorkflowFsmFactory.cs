using AdminCore.FsmWorkflow.FsmMachines;
using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;

namespace AdminCore.FsmWorkflow.Factory
{
    public class WorkflowFsmFactory : IWorkflowFsmFactory<ILeaveWorkflow>
    {
        public ILeaveWorkflow GetWorkflowPto(WorkflowStateData workflowStateData)
        {
            return new WorkflowFsmPto(workflowStateData);
        }

        public ILeaveWorkflow GetWorkflowWfh(WorkflowStateData workflowStateData)
        {
            return new WorkflowFsmWfh(workflowStateData);
        }
    }
}