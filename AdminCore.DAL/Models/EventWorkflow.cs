using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("event_workflow")]
  public class EventWorkflow
  {
    [Key]
    [Column("event_workflow_id")]
    public int EventWorkflowId { get; set; }

    [Column("event_id")]
    public int EventId { get; set; }

    public virtual Event Event { get; set; }
    
    [Column("workflow_state")]
    public int WorkflowState { get; set; }
    
    public virtual ICollection<EmployeeApprovalResponse> EventWorkflowApprovalStatuses { get; set; }
    public virtual ICollection<EventWorkflowResponder> EventWorkflowResponders { get; set; }
  }
}