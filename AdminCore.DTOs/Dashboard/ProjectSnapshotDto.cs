using System.Collections.Generic;

namespace AdminCore.DTOs.Dashboard
{
  public class ProjectSnapshotDto
  {
    public int ProjectId { get; set; }

    public string ProjectName { get; set; }

    public ICollection<TeamSnapshotDto> Teams { get; set; }
  }
}
