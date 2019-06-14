using System;
using AdminCore.WebApi.Models.Base;
using AdminCore.WebApi.Models.SystemUser;

namespace AdminCore.WebApi.Models.Employee
{
  public class EmployeeViewModel : ViewModel
  {
    public int EmployeeId { get; set; }

    public int SystemUserId { get; set; }

    public SystemUserViewModel SystemUser { get; set; }

    public string CountryDescription { get; set; }

    public int CountryId { get; set; }

    public string Email { get; set; }

    public int EmployeeStatusId { get; set; }

    public string Forename { get; set; }

    public DateTime StartDate { get; set; }

    public string Surname { get; set; }

    public int TotalHolidays { get; set; }
  }
}
