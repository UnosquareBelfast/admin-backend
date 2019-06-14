using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.SystemUser
{
  public class SystemUserRoleViewModel : ViewModel
  {
    public int SystemUserRoleId { get; set; }

    public string Description { get; set; }
  }
}
