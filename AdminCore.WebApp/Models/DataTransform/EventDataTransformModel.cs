using System;
using System.Collections.Generic;
using AdminCore.Common.Attributes.DataEtl;
using AdminCore.Constants.Enums;

namespace AdminCore.WebApi.Models
{
    public class EventDataTransformModel
    {
        [CsvRecordField(Name = "Event Id", ColumnPosition = 1)]
        public int EventId { get; set; }
        [CsvRecordField(Name = "Date Created", ColumnPosition = 2)]
        public DateTime DateCreated { get; set; }
        [CsvRecordField(Name = "Event Status Id", ColumnPosition = 3)]
        [TypeConverter(ConverterType = typeof(TypeToStringConverter<EventStatuses>))]
        public int EventStatusId { get; set; }
        [CsvRecordField(Name = "Event Type", ColumnPosition = 4)]
        public int EventTypeId { get; set; }
        [CsvRecordField(Name = "Last Modified", ColumnPosition = 5)]
        public DateTime LastModified { get; set; }
        [CsvRecordField(Name = "Event Messages", ColumnPosition = 6)]
        [TypeConverter(ConverterType = typeof(ListToStringConverter<string>))]
        public ICollection<string> EventMessages { get; set; }
        [CsvRecordField(Name = "Employee Id", ColumnPosition = 7)]
        public int EmployeeId { get; set; }
        [CsvRecordField(Name = "Employee", ColumnPosition = 8)]
        public string Employee { get; set; }
        [CsvRecordField(Name = "Event Start Date", ColumnPosition = 9)]
        public DateTime EventDateStart { get; set; }
        [CsvRecordField(Name = "Event End Date", ColumnPosition = 10)]
        public DateTime EventDateEnd { get; set; }
    }
}