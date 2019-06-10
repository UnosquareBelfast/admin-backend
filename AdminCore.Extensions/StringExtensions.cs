using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Extensions
{
    public static class StringExtensions
    {
        public static IList<string> SplitStringBySeparatorAndTakeRange(this string splitString, char separator, int rangeMin)
        {
            var splitList = splitString.Split(separator).ToList();
            return splitList.GetRange(rangeMin, splitList.Count - rangeMin);
        }
    }
}
