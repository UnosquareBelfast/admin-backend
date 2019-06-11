using AdminCore.DTOs.Team;
using System;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs
{
  public class SystemUserDto
  {
    public int SystemUserId { get; set; }

    public int SystemUserTypeId { get; set; }

    public SystemUserTypeDto SystemUserType { get; set; }
  }
}
