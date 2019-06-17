using AdminCore.WebApi.Models.Team;
using System;
using AdminCore.WebApi.Models.Base;
using AdminCore.WebApi.Models.SystemUser;

namespace AdminCore.WebApi.Models.Contract
{
  public class ContractViewModel : ViewModel
  {
    public int ContractId { get; set; }

    public TeamViewModel Team { get; set; }

    public string ClientName { get; set; }

    public int EmployeeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int SystemUserRoleId { get; set; }

    public SystemUserRoleViewModel SystemUserRole { get; set; }
  }
}
