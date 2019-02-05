using System;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Employee
{
  public class RegisterEmployeeViewModel : ViewModel
  {
    public int CountryId { get; set; }

    public int EmployeeRoleId { get; set; }

    public int EmployeeStatusId { get; set; }

    public DateTime StartDate { get; set; }
  }
}