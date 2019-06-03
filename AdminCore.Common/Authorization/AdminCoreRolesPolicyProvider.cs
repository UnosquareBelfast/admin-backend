using System;
using System.Linq;
using System.Threading.Tasks;
using AdminCore.Constants.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesPolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                var splitList = policyName.Split(separator).ToList();
                splitList = splitList.GetRange(1, splitList.Count - 1);

                var employeeRolesList = splitList.Select(x => (EmployeeRoles) Enum.Parse(typeof(EmployeeRoles), x)).ToList();

                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new AdminCoreRolesRequirement(employeeRolesList));
                return Task.FromResult(policy.Build());
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }
}
