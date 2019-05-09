using AdminCore.DTOs.Employee;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.DAL.Models
{
    public class EventWorkflowResponderDto
    {
        public int EventWorkflowResponderId { get; set; }
        
        public int EventWorkflowId { get; set; }
        public EventWorkflowDto EventWorkflow { get; set; }
        
        public int EmployeeRoleId { get; set; }
        public EmployeeRoleDto EmployeeRole { get; set; }
    }
}