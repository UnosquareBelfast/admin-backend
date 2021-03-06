﻿using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Dashboard
{
  public class EmployeeSnapshotViewModel : ViewModel
  {
    public int EmployeeId { get; set; }

    public string Forename { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string Location { get; set; }
  }
}