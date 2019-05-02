using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("event_workflow_responder")]
    public class EventWorkflowResponder
    {
        [Key]
        [Column("event_workflow_responder_id")]
        public int EventWorkflowResponderId { get; set; }
        
        [Column("event_workflow_id")]
        public int EventWorkflowId { get; set; }
        [ForeignKey("EventWorkflowId")]
        public EventWorkflow EventWorkflow { get; set; }
        
        [Column("employee_role_id")]
        public int EmployeeRoleId { get; set; }
        [ForeignKey("EmployeeRoleId")]
        public EmployeeRole EmployeeRole { get; set; }
    }
}