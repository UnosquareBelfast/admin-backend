using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("event_workflow")]
  public class EventWorkflow : ISoftDeletable
  {
    [Key]
    [Column("event_workflow_id")]
    public int EventWorkflowId { get; set; }

    [Column("event_id")]
    public int EventId { get; set; }
    
    [StringLength(200)]
    [Column("workflow_serialized_state")]
    public string WorkflowSerializedState { get; set; }

    public virtual Event Event { get; set; }
  }
}