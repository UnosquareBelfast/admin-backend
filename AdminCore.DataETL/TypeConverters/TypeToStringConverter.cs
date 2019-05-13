namespace AdminCore.Common.Attributes.DataEtl
{
    /// <summary>
    /// Converts a type to a string using the ToString() method.
    /// </summary>
    public class TypeToStringConverter<T> : ITypeConverter
    {
        public object ConvertTo(object value)
        {
            string stringValue = ((T)value)?.ToString();
            return stringValue;
        }
    }
}