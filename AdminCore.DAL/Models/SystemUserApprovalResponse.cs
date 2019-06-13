using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("system_user_approval_response")]
  public class SystemUserApprovalResponse
  {
    [Key]
    [Column("system_user_approval_response_id")]
    public int SystemUserApprovalResponseId { get; set; }

    [Column("response_sent_date")]
    public DateTime ResonseSentDate { get; set; }

    [Column("system_user_role_id")]
    public int SystemUserRoleId { get; set; }
    [ForeignKey("SystemUserRoleId")]
    public virtual SystemUserRole SystemUserRole { get; set; }

    [Column("event_status_id")]
    public int EventStatusId { get; set; }
    [ForeignKey("EventStatusId")]
    public virtual EventStatus EventStatus { get; set; }

    [Column("event_workflow_id")]
    public int EventWorkflowId { get; set; }
    [ForeignKey("EventWorkflowId")]
    public virtual EventWorkflow EventWorkflow { get; set; }

    [Column("system_user_id")]
    public virtual int SystemUserId { get; set; }
    [ForeignKey("SystemUserId")]
    public virtual SystemUser SystemUser { get; set; }
  }
}
