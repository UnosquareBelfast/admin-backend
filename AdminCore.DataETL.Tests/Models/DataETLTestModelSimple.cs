using System;
using System.Collections.Generic;
using AdminCore.DataETL.Attributes;

namespace AdminCore.DataETL.Tests.Models
{
    public class DataETLTestModelSimple
    {
        [ExportableRecordField(Name = "Field1", ColumnPosition = 1)]
        public int Field_Int { get; set; }
        [ExportableRecordField(Name = "Field2", ColumnPosition = 2)]
        public DateTime Field_DateTime { get; set; }
        [ExportableRecordField(Name = "Field3", ColumnPosition = 3)]
        public string Field_String { get; set; }
        [ExportableRecordField(Name = "Field4", ColumnPosition = 4)]
        public bool Field_Bool { get; set; }
        [ExportableRecordField(Name = "Field5", ColumnPosition = 5)]
        public float  Field_Float { get; set; }
    }
}
