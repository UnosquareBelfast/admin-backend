using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AdminCore.Common.Authorization
{
    public class AdminCoreRolesAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "AdminCoreRoles";

        private const char separator = '_';

        public AdminCoreRolesAttribute(params string[] adminCoreEmployeeRoles) =>
//            Policy = String.Join(',', adminCoreEmployeeRoles);
            AdminCoreRoles = adminCoreEmployeeRoles;

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
