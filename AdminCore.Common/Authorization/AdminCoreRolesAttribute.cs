using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        public AdminCoreRolesAttribute(params string[] adminCoreEmployeeRoles) => Policy = $"{POLICY_PREFIX}{separator}{string.Join(separator, adminCoreEmployeeRoles)}";
    }
}
