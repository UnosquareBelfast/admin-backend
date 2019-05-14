using System;
using System.Collections.Generic;
using AdminCore.DataETL.Attributes;
using AdminCore.DataETL.TypeConverters;

namespace AdminCore.DataETL.Tests.Models
{
    public class DataETLTestModelTypeToStringConverter
    {
        [ExportableRecordField(Name = "Field1", ColumnPosition = 1)]
        [TypeConverter(ConverterType = typeof(TypeToStringConverter<DataETLTestEnum>))]
        public int DataETLTestEnum1 { get; set; }
    }
}
