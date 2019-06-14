using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("event_request")]
    public class EventRequest
    {
        [Key]
        [Column("event_request_id")]
        public int EventRequestId { get; set; }

        [Column("request_type_id")]
        public int RequestTypeId { get; set; }

        [ForeignKey("RequestTypeId")]
        public EventRequestType RequestType { get; set; }

        [Column("event_date_id")]
        public int EventDateId { get; set; }

        [ForeignKey("EventDateId")]
        public EventDate EventDate { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("hash")]
        public string Hash { get; set; }

        [Column("time_created")]
        public DateTime TimeCreated { get; set; }

        [Column("time_expires")]
        public DateTime TimeExpires { get; set; }

        [Column("approved")]
        public bool Approved { get; set; }

        [Column("auto_approved")]
        public bool AutoApproved { get; set; }
    }
}
