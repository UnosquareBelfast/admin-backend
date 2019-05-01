using System.ComponentModel.DataAnnotations.Schema;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.DAL.Models
{
  public class EventWorkflowApprovalStatusDto
  {
    public int EmployeeApprovalStatusId { get; set; }

    public int EmployeeRoleId { get; set; }

    public virtual EmployeeRoleDto EmployeeRole { get; set; }
    
    public int ApprovalStatusId { get; set; }

    public virtual ApprovalStatusDto ApprovalStatus { get; set; }
    
    public int EventWorkflowId { get; set; }

    public virtual EventWorkflowDto EventWorkflow { get; set; }
    
    public int EmployeeId { get; set; }

    public virtual EmployeeDto Employee { get; set; }
  }
}