using System;
using System.Collections.Generic;

namespace AdminCore.Common.Attributes.DataEtl
{
    /// <summary>
    /// Converts a type to a string using the ToString() method.
    /// </summary>
    public class ListToStringConverter<T> : ITypeConverter
    {
        private string _defaultIfNull = "Empty";

        public object ConvertTo(object value)
        {
            if (value == null)
            {
                return _defaultIfNull;
            }
            
            var list = (List<T>) value;
            
            if (list.Count == 0)
            {
                return _defaultIfNull;
            }
            return string.Join($"{Environment.NewLine}", list);
        }
    }
}