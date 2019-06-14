using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.SystemUser
{
  public class SystemUserViewModel : ViewModel
  {
    public int SystemUserId { get; set; }

    public int SystemUserRoleId { get; set; }

    public SystemUserRoleViewModel SystemUserRole { get; set; }
  }
}
