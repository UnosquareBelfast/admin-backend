using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Team
{
  public class CreateTeamViewModel : ViewModel
  {
    public int ClientId { get; set; }
    public string TeamName { get; set; }
  }
}