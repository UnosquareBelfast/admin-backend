using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models.EventRequest
{
    [Table("event_request_response_type")]
    public class EventRequestResponseType
    {
        [Key]
        [Column("request_response_type_id")]
        public int RequestResponseTypeId { get; set; }

        [Column("type_value")]
        public string Type { get; set; }
    }
}