using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Services.Authorization
{
    public class AdminCoreRolesAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        public AdminCoreRolesAttribute(IList<string> adminCoreRoles) => AdminCoreRoles = adminCoreRoles;

        public IList<string> AdminCoreRoles
        {
            get
            {
                var splitList = Policy.Split(separator).ToList();
                return splitList.GetRange(1, splitList.Count - 1);
            }
            set => Policy = $"{POLICY_PREFIX}{separator}{string.Join(separator, value)}";
        }
    }
}
