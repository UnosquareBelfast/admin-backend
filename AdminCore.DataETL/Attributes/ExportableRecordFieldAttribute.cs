using System;

namespace AdminCore.DataETL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportableRecordFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public int ColumnPosition { get; set; }
    }
}