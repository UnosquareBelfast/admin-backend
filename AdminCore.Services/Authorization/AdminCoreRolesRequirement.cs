using System.Collections.Generic;
using AdminCore.Constants.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Services.Authorization
{
    public class AdminCoreRolesRequirement : IAuthorizationRequirement
    {
        public IList<EmployeeRoles> UserRoles { get; }

        public AdminCoreRolesRequirement(IList<EmployeeRoles> userRoles)
        {
            UserRoles = userRoles;
        }
    }
}
