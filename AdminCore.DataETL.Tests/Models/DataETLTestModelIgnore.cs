using System;
using System.Collections.Generic;
using AdminCore.DataETL.Attributes;

namespace AdminCore.DataETL.Tests.Models
{
    public class DataETLTestModelIgnore
    {
        [ExportableRecordField(Name = "Field1", ColumnPosition = 1)]
        public string Field_String1 { get; set; }
        [IgnoreRecordField]
        public string Field_String2 { get; set; }
        [ExportableRecordField(Name = "Field3", ColumnPosition = 2)]
        public string Field_String3 { get; set; }
    }
}
