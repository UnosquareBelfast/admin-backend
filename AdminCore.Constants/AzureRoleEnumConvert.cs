using System;
using AdminCore.Constants.Enums;

namespace AdminCore.Constants
{
  public static class AzureRoleEnumConvert
  {
    public static EmployeeRoles ConvertToEmployeeRoles(string azureRole)
    {
      switch (azureRole)
      {
        case AzureRoleConstants.SystemAdministrator:
          return EmployeeRoles.SystemAdministrator;
        case AzureRoleConstants.TeamLeader:
          return EmployeeRoles.TeamLeader;
        case AzureRoleConstants.User:
          return EmployeeRoles.User;
        default:
          throw new InvalidOperationException($"A matching EmployeeRoles enum does not exist for {azureRole}");
      }
    }
  }
}
