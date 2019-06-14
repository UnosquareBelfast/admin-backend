using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models.EventRequest
{
    [Table("event_request")]
    public class EventRequest
    {
        [Key]
        [Column("event_request_id")]
        public int EventRequestId { get; set; }

        [Column("request_response_type_id")]
        public int RequestResponseTypeId { get; set; }

        [ForeignKey("RequestResponseTypeId")]
        public EventRequestResponseType RequestResponseType { get; set; }

        [Column("request_type_id")]
        public int RequestTypeId { get; set; }

        [ForeignKey("RequestTypeId")]
        public EventRequestType RequestType { get; set; }

        [Column("request_status_id")]
        public int RequestStatusId { get; set; }

        [ForeignKey("RequestStatusId")]
        public EventRequestStatus RequestStatus { get; set; }

        [Column("salt")]
        public string Salt { get; set; }

        [Column("hash")]
        public string Hash { get; set; }
    }
}
