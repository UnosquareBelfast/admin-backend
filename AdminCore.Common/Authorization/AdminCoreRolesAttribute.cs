using AdminCore.Constants;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesAttribute : AuthorizeAttribute
    {
        public AdminCoreRolesAttribute(params EmployeeRoles[] adminCoreEmployeeRoles) => Policy =
            $"{PolicyProviderConstants.PolicyPrefixAdminCoreRoles}{PolicyProviderConstants.SeparatorAdminCoreRoles}{string.Join(PolicyProviderConstants.SeparatorAdminCoreRoles, adminCoreEmployeeRoles)}";
    }
}
