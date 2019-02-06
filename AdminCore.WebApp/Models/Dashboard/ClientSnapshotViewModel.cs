using System.Collections.Generic;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Dashboard
{
  public class ClientSnapshotViewModel : ViewModel
  {
    public int ClientId { get; set; }

    public string ClientName { get; set; } 

    public ICollection<TeamSnapshotViewModel> Teams { get; set; }
  }
}
