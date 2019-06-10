using System.Collections.Generic;
using System.Linq;
using AdminCore.Constants;

namespace AdminCore.Extensions
{
    public static class StringExtensions
    {
        public static List<string> GetContentFromPolicy(this string policyName)
        {
            var splitList = policyName.Split(PolicyProviderConstants.Separator).ToList();
            return splitList.GetRange(1, splitList.Count - 1);
        }
    }
}
