﻿using System;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Employee
{
  public class UpdateEmployeeViewModel : ViewModel
  {
    public int CountryId { get; set; }

    public string Email { get; set; }

    public int EmployeeId { get; set; }

    public int EmployeeRoleId { get; set; }

    public int EmployeeStatusId { get; set; }

    public string Forename { get; set; }

    public DateTime StartDate { get; set; }

    public string Surname { get; set; }

    public double TotalHolidays { get; set; }
  }
}