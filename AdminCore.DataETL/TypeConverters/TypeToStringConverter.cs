using System;
using System.Globalization;
using ChoETL;

namespace AdminCore.DataETL.TypeConverters
{
    public class TypeToStringConverter<T> : IChoValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string enumValue = ((T)value).ToString();
            return enumValue;
        }
    }
}