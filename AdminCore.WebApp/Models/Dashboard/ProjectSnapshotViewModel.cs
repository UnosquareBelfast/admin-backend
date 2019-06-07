using System.Collections.Generic;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Dashboard
{
  public class ProjectSnapshotViewModel : ViewModel
  {
    public int ProjectId { get; set; }

    public string ProjectName { get; set; }

    public ICollection<TeamSnapshotViewModel> Teams { get; set; }
  }
}
