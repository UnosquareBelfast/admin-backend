using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.FsmWorkflow.EnumConstants;

namespace AdminCore.FsmWorkflow
{
    public interface IFsmWorkflowHandler
    {
        EventWorkflow CreateEventWorkflow(EventTypes eventType, string teamLead, string client, string cse);

        void FireLeaveResponded(EventTypes eventType, string serializedFsm, ApprovalState approvalState, string responderName);
    }
}