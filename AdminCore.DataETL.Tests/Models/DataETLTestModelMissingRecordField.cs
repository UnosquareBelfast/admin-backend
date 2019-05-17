using System;
using System.Collections.Generic;
using AdminCore.DataETL.Attributes;

namespace AdminCore.DataETL.Tests.Models
{
    public class DataETLTestModelMissingRecordField
    {
        [ExportableRecordField(Name = "Field1", ColumnPosition = 1)]
        public int Field_Int1 { get; set; }
        public int Field_Int2 { get; set; }
    }
}
