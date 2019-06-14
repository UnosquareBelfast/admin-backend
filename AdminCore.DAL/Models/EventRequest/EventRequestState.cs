using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models.EventRequest
{
    [Table("event_request_state")]
    public class EventRequestState
    {
        [Key]
        [Column("request_state_id")]
        public int RequestStateId { get; set; }

        [Column("event_id")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Column("system_user_id")]
        public int SystemUserId { get; set; }

        [ForeignKey("SystemUserId")]
        public SystemUser SystemUser { get; set; }

        [Column("event_request_id")]
        public int EventRequestId { get; set; }

        [ForeignKey("EventRequestId")]
        public EventRequest EventRequest { get; set; }

        [Column("time_expires")]
        public DateTime TimeExpires { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("auto_approved")]
        public bool AutoApproved { get; set; }
    }
}
