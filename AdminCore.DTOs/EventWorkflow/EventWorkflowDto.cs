
using System.Collections.Generic;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;

namespace AdminCore.DTOs.EventWorkflow
{
    public class EventWorkflowDto
    {
        public int EventWorkflowId { get; set; }

        public int EventId { get; set; }

        public virtual EventDto Event { get; set; }
    
        public int WorkflowState { get; set; }
        
        public virtual ICollection<EmployeeApprovalResponseDto> EventWorkflowApprovalStatuses { get; set; }
    }
}