using System;
using AdminCore.Constants.Enums;

namespace AdminCore.DataETL.Models
{
    public class EventMessageChoEtl
    {
        public int EventMessageId { get; set; }
        public string Message { get; set; }
        public int EventMessageTypeId { get; set; }
        public DateTime LastModified { get; set; }
        
        public override string ToString()
        {
            return $"{(EventMessageTypes)EventMessageTypeId}: {Message}";
        }
    }
}