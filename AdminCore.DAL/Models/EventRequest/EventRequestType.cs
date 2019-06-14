using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models.EventRequest
{
    [Table("event_request_type")]
    public class EventRequestType
    {
        [Key]
        [Column("request_type_id")]
        public int RequestTypeId { get; set; }

        [Column("type_value")]
        public string Type { get; set; }

        [Column("life_span")]
        public int LifeSpan { get; set; }

        [Column("locked")]
        public bool Locked { get; set; }
    }
}
