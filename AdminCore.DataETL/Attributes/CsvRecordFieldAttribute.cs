using System;

namespace AdminCore.Common.Attributes.DataEtl
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CsvRecordFieldAttribute : Attribute
    {
        public string Name { get; set; }
        public int ColumnPosition { get; set; }
    }
}