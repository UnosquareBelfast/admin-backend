using System;

namespace AdminCore.DataETL.Models
{
    public class EventDateChoEtl
    {
        public int EventDateId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHalfDay { get; set; }
        
        public override string ToString()
        {
            return $"{StartDate} => {EndDate}";
        }
    }
}