
using System;
using System.Collections.Generic;
using AdminCore.DataETL.Attributes;
using AdminCore.DataETL.TypeConverters;

namespace AdminCore.DataETL.Tests.Models
{
    public class DataETLTestModelListToStringConverter
    {
        [ExportableRecordField(Name = "Field1", ColumnPosition = 1)]
        [TypeConverter(ConverterType = typeof(ListToStringConverter<string>))]
        public ICollection<string> StringCollection { get; set; }
    }
}
