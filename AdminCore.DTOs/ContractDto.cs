using AdminCore.DTOs.Team;
using System;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs
{
  public class ContractDto
  {
    public int ContractId { get; set; }

    public int TeamId { get; set; }

    public TeamDto Team { get; set; }

    public string ClientName { get; set; }

    public string ProjectName { get; set; }

    public int EmployeeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int SystemUserRoleId { get; set; }

    public EmployeeRoleDto SystemUserRole { get; set; }
  }
}
