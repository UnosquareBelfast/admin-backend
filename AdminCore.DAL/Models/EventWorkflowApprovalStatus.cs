using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("event_workflow_approval_status")]
  public class EventWorkflowApprovalStatus : ISoftDeletable
  {
    [Key]
    [Column("employee_approval_status_id")]
    public int EmployeeApprovalStatusId { get; set; }

    [Column("employee_role_id")]
    public int EmployeeRoleId { get; set; }

    [ForeignKey("EmployeeRoleId")]
    public virtual EmployeeRole EmployeeRole { get; set; }
    
    [Column("approval_status_id")]
    public int ApprovalStatusId { get; set; }

    [ForeignKey("ApprovalStatusId")]
    public virtual ApprovalStatus ApprovalStatus { get; set; }
    
    [Column("event_workflow_id")]
    public int EventWorkflowId { get; set; }

    [ForeignKey("EventWorkflowId")]
    public virtual EventWorkflow EventWorkflow { get; set; }
    
    [Column("employee_id")]
    public virtual int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public virtual Employee Employee { get; set; }
  }
}