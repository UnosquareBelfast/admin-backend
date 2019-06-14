using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("event_request_type")]
    public class EventRequestType
    {
        [Key]
        [Column("request_type_id")]
        public int RequestTypeId { get; set; }

        [Column("request_description")]
        public string RequestDescription { get; set; }

        [Column("request_life_cycle")]
        public int RequestLifeCycle { get; set; }
    }
}
