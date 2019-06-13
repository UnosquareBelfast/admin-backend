using System;
using AdminCore.WebApi.Models.Base;
using AdminCore.WebApi.Models.SystemUser;

namespace AdminCore.WebApi.Models.Employee
{
  public class RegisterEmployeeViewModel : ViewModel
  {
    public int CountryId { get; set; }

    public int SystemUserId { get; set; }

    public SystemUserViewModel SystemUser { get; set; }

    public int EmployeeStatusId { get; set; }

    public DateTime StartDate { get; set; }
  }
}
