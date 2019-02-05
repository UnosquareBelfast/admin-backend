using System;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Contract
{
  public class CreateContractViewModel : ViewModel
  {
    public int TeamId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
  }
}