using System;
using System.Collections.Generic;
using System.Globalization;
using ChoETL;

namespace AdminCore.DataETL.TypeConverters
{
    public class ListConverter<T> : IChoValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = (List<T>) value; 
            return string.Join($", ", col);
        }
    }
}