using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("event_type_required_responders")]
    public class EventTypeRequiredResponders
    {
        [Key]
        [Column("event_type_required_responders_id")]
        public int EventTypeRequiredRespondersId { get; set; }
        
        [Column("event_type_id")]
        public int EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventType EventType { get; set; }
        
        [Column("employee_role_id")]
        public int EmployeeRoleId { get; set; }
        [ForeignKey("EmployeeRoleId")]
        public EmployeeRole EmployeeRole { get; set; }
    }
}