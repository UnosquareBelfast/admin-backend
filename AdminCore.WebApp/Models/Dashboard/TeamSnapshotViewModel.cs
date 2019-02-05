using System.Collections.Generic;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Dashboard
{
  public class TeamSnapshotViewModel : ViewModel
  {
    public int TeamId { get; set; }

    public string TeamName { get; set; }

    public ICollection<EmployeeSnapshotViewModel> Employees { get; set; }
  }
}