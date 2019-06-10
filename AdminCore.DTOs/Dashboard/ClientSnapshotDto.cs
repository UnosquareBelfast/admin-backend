using System.Collections.Generic;

namespace AdminCore.DTOs.Dashboard
{
  public class ClientSnapshotDto
  {
    public int ClientId { get; set; }

    public string ClientName { get; set; }

    public ICollection<ProjectSnapshotDto> Projects { get; set; }
  }
}
