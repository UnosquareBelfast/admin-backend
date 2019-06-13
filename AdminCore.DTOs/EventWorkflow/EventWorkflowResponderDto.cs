using AdminCore.DTOs.Employee;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs.EventWorkflow
{
    public class EventWorkflowResponderDto
    {
        public int EventWorkflowResponderId { get; set; }
        
        public int EventWorkflowId { get; set; }
        public EventWorkflowDto EventWorkflow { get; set; }
        
        public int EmployeeRoleId { get; set; }
        public SystemUserRoleDto SystemUserRole { get; set; }
    }
}