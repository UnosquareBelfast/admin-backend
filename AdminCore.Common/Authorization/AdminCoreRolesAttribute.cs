using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        public AdminCoreRolesAttribute(params EmployeeRoles[] adminCoreEmployeeRoles) => Policy = $"{POLICY_PREFIX}{separator}{string.Join(separator, adminCoreEmployeeRoles)}";
    }
}
