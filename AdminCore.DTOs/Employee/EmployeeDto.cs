using System;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs.Employee
{
  public class EmployeeDto
  {
    public int EmployeeId { get; set; }

    public int SystemUserId { get; set; }

    public int CountryId { get; set; }

    public string Email { get; set; }

    public string Forename { get; set; }

    public DateTime StartDate { get; set; }

    public int EmployeeStatusId { get; set; }

    public string Surname { get; set; }

    public SystemUserDto SystemUser { get; set; }

    public int TotalHolidays { get; set; }
  }
}
