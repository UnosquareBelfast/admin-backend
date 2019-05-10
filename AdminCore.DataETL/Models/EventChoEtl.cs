using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdminCore.Constants.Enums;
using AdminCore.DataETL.TypeConverters;
using ChoETL;

namespace AdminCore.DataETL.Models
{
    [ChoCSVFileHeader]
    [ChoCSVRecordObject(ErrorMode = ChoErrorMode.ThrowAndStop)]
    public class EventChoEtl
    {
        [ChoCSVRecordField(1, FieldName = "Event Id")]
        [Required]
        public int EventId { get; set; }
        [ChoCSVRecordField(2, FieldName = "Date Created")]
        public DateTime DateCreated { get; set; }

        [ChoCSVRecordField(3, FieldName = "Event Status")]
        [ChoTypeConverter(typeof(TypeToStringConverter<EventStatuses>))]
        public int EventStatusId { get; set; }

        [ChoCSVRecordField(4, FieldName = "Event Type")]
        [ChoTypeConverter(typeof(TypeToStringConverter<EventTypes>))]
        public int EventTypeId { get; set; }

        [ChoCSVRecordField(5, FieldName = "Date Last Modified")]
        public DateTime LastModified { get; set; }
        [ChoCSVRecordField(6, FieldName = "Event Messages")]
        [ChoTypeConverter(typeof(ListConverter<EventMessageChoEtl>))]
        public ICollection<EventMessageChoEtl> EventMessages { get; set; }
        
        [ChoCSVRecordField(7, FieldName = "Employee Id")]
        public int EmployeeId { get; set; }
        [ChoCSVRecordField(8, FieldName = "Employee Name")]
        public string Employee { get; set; }

//        [ChoCSVRecordField(9, FieldName = "Event Dates")]
//        [ChoTypeConverter(typeof(ListConverter<EventDateChoEtl>))]
//        public ICollection<EventDateChoEtl> EventDates { get; set; }

        [ChoCSVRecordField(9, FieldName = "Start Date")]
        public DateTime EventDateStart { get; set; }
        
        [ChoCSVRecordField(10, FieldName = "End Date")]
        public DateTime EventDateEnd { get; set; }
    }
}