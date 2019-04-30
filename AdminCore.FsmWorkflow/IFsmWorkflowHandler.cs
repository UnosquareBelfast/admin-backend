using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;

namespace AdminCore.FsmWorkflow
{
    public interface IFsmWorkflowHandler
    {
        EventWorkflow CreateEventWorkflow(EventTypes eventType, string teamLead, string client, string cse);
    }
}