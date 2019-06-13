namespace AdminCore.DTOs.SystemUser
{
  public class SystemUserDto
  {
    public int SystemUserId { get; set; }

    public int SystemUserRoleId { get; set; }

    public SystemUserRoleDto SystemUserRole { get; set; }
  }
}
