using AdminCore.Constants.Enums;

namespace AdminCore.Common
{
    public class WorkflowFsmStateInfo
    {
        public bool Completed { get; set; }
        public EventStatuses CurrentEventStatuses { get; set; }
        public string Message { get; set; }
        
        public WorkflowFsmStateInfo(bool completed, EventStatuses currentEventStatuses, string message)
        {
            Completed = completed;
            CurrentEventStatuses = currentEventStatuses;
            Message = message;
        }
    }
}