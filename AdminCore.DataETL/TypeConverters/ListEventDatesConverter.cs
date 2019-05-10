using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdminCore.DataETL.Models;
using ChoETL;

namespace AdminCore.DataETL.TypeConverters
{
    public class ListEventDatesConverter : IChoValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = (List<EventDateChoEtl>) value;
            
            var first = col.FirstOrDefault();
            var last = col.LastOrDefault();
            
            return $"{first.StartDate} ";
        }
    }
}