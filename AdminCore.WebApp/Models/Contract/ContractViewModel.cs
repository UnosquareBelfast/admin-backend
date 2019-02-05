using AdminCore.WebApi.Models.Team;
using System;
using AdminCore.WebApi.Models.Base;

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
  }
}