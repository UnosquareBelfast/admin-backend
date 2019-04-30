
using AdminCore.DTOs.Event;

namespace AdminCore.DTOs.EventWorkflow
{
    public class EventWorkflowDto
    {
        public int WorkflowId { get; set; }

        public int EventId { get; set; }
    
        public string WorkflowSerializedState { get; set; }

        public virtual EventDto Event { get; set; }
    }
}