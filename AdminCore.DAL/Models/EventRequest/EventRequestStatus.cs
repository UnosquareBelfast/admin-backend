using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models.EventRequest
{
    [Table("event_request_status")]
    public class EventRequestStatus
    {
        [Key]
        [Column("request_status_id")]
        public int RequestStatusId { get; set; }

        [Column("type_value")]
        public string Type { get; set; }
    }
}
