using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using AdminCore.Constants;
using AdminCore.Constants.Enums;

namespace AdminCore.Common.Authorization
{
  public static class AzureRoleEnumConvert
  {
    public static EmployeeRoles ConvertToEmployeeRoles(string azureRole)
    {
      var type = typeof(EmployeeRoles);
      var values = Enum.GetValues(type);

      foreach (int val in values)
      {
          var memInfo = type.GetMember(type.GetEnumName(val));

          if (memInfo[0]
            .GetCustomAttributes(typeof(AzureRoleAttribute), false)
            .FirstOrDefault() is AzureRoleAttribute azureRoleAttribute)
          {
            if (azureRoleAttribute.AzureRoleName == azureRole)
            {
              return (EmployeeRoles) val;
            }
          }
      }

      throw new InvalidOperationException($"A matching EmployeeRoles enum does not exist for {azureRole}");
    }
  }
}
