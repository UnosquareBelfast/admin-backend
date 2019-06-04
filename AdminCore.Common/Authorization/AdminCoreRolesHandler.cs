using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AdminCore.Constants;
using AdminCore.Constants.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesHandler : AuthorizationHandler<AdminCoreRolesRequirement>
    {
        private readonly Predicate<Claim> claimPredicate = claim => claim.Type == UserDetailsConstants.Role;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminCoreRolesRequirement requirement)
        {
            if (!context.User.HasClaim(claimPredicate))
            {
                return Task.CompletedTask;
            }

//            var parsed = Enum.TryParse<EmployeeRoles>(context.User.FindFirst(claimPredicate).Value, out var userRole);
            var userRole = ConvertToEmployeeRoles(context.User.FindFirst(claimPredicate).Value);

            if (requirement.UserRoles.Contains(userRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private EmployeeRoles ConvertToEmployeeRoles(string roleName)
        {
            switch(roleName)
            {
                case "Admin":
                    return EmployeeRoles.SystemAdministrator;
                default:
                    return EmployeeRoles.User;
            }
        }
    }
}
