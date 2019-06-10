using System;

namespace AdminCore.Constants
{
    public class AzureRoleAttribute : Attribute
    {
        public string AzureRoleName { get; }

        public AzureRoleAttribute(string azureRoleName)
        {
            AzureRoleName = azureRoleName;
        }
    }
}
