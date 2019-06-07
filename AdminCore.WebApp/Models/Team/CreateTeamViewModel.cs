using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Team
{
  public class CreateTeamViewModel : ViewModel
  {
    public int ProjectId { get; set; }
    public string TeamName { get; set; }
  }
}
