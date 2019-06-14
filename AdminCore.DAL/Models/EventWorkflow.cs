using System.Collections.Generic;
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
    
    [Column("workflow_state")]
    public int WorkflowState { get; set; }
    
    public virtual ICollection<SystemUserApprovalResponse> EventWorkflowApprovalResponses { get; set; }
  }
}
