using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs.EventWorkflow
{
  public class SystemUserApprovalResponseDto
  {
    public int SystemUserApprovalResponseId { get; set; }

    public int SystemUserRoleId { get; set; }

    public virtual SystemUserRoleDto SystemUserRole { get; set; }

    public int EventStatusId { get; set; }

    public virtual EventStatusDto EventStatus { get; set; }

    public int EventWorkflowId { get; set; }

    public virtual EventWorkflowDto EventWorkflow { get; set; }

    public int SystemUserId { get; set; }

    public virtual SystemUserDto SystemUser { get; set; }
  }
}
