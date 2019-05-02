using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("employee_approval_response")]
  public class EmployeeApprovalResponse
  {
    [Key]
    [Column("employee_approval_response_id")]
    public int EmployeeApprovalResponseId { get; set; }

    [Column("employee_role_id")]
    public int EmployeeRoleId { get; set; }

    [ForeignKey("EmployeeRoleId")]
    public virtual EmployeeRole EmployeeRole { get; set; }
    
    [Column("event_status_id")]
    public int EventStatusId { get; set; }

    [ForeignKey("EventStatusId")]
    public virtual EventStatus EventStatus { get; set; }
    
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