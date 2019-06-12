using AdminCore.DTOs.Employee;

namespace AdminCore.DTOs.EventWorkflow
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