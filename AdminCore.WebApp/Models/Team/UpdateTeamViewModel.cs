using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Team
{
  public class UpdateTeamViewModel : ViewModel
  {
    public int ClientId { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; }
  }
}