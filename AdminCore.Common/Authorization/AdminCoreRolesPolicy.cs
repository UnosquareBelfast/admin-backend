using System;
using System.Linq;
using System.Threading.Tasks;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesPolicy : DefaultAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        private readonly IConfiguration _configuration;
        private readonly AuthorizationOptions _options;

        public AdminCoreRolesPolicy(IOptions<AuthorizationOptions> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            _options = options.Value;
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                var splitList = policyName.Split(separator).ToList();
                splitList = splitList.GetRange(1, splitList.Count - 1);

                var employeeRolesList = splitList.Select(x => (EmployeeRoles) Enum.Parse(typeof(EmployeeRoles), x)).ToList();

                policy = new AuthorizationPolicyBuilder().AddRequirements(new AdminCoreRolesRequirement(employeeRolesList)).Build();
                _options.AddPolicy(policyName, policy);
            }

//            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
//            {
//                var splitList = policyName.Split(separator).ToList();
//                splitList = splitList.GetRange(1, splitList.Count - 1);
//
//                var employeeRolesList = splitList.Select(x => (EmployeeRoles) Enum.Parse(typeof(EmployeeRoles), x)).ToList();
//
//                policy = new AuthorizationPolicyBuilder();
//                policy.AddRequirements(new AdminCoreRolesRequirement(employeeRolesList));
//                return Task.FromResult(policy.Build());
//            }

            return policy;
        }
    }
}
