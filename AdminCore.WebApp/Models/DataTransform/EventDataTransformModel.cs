using System;
using AdminCore.Constants.Enums;
using AdminCore.DataETL.Attributes;

namespace AdminCore.WebApi.Models.DataTransform
{
    public class EventDataTransformModel
    {
        [ExportableRecordField(Name = "Event Id", ColumnPosition = 1)]
        public int EventId { get; set; }
        [ExportableRecordField(Name = "Date Created", ColumnPosition = 2)]
        public DateTime DateCreated { get; set; }
        [ExportableRecordField(Name = "Event Status Id", ColumnPosition = 3)]
        public EventStatuses EventStatus { get; set; }
        [ExportableRecordField(Name = "Event Type", ColumnPosition = 4)]
        public int EventTypeId { get; set; }
        [ExportableRecordField(Name = "Last Modified", ColumnPosition = 5)]
        public DateTime LastModified { get; set; }
        [ExportableRecordField(Name = "Event Messages", ColumnPosition = 6)]
        public string EventMessages { get; set; }
        [ExportableRecordField(Name = "Employee Id", ColumnPosition = 7)]
        public int EmployeeId { get; set; }
        [ExportableRecordField(Name = "Employee", ColumnPosition = 8)]
        public string Employee { get; set; }
        [ExportableRecordField(Name = "Event Start Date", ColumnPosition = 9)]
        public DateTime EventDateStart { get; set; }
        [ExportableRecordField(Name = "Event End Date", ColumnPosition = 10)]
        public DateTime EventDateEnd { get; set; }
    }
}
