using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("event_type_required_responders")]
    public class EventTypeRequiredResponders
    {
        // Composite primary key declared using fluent api in DbContext.

        [Column("event_type_id")]
        public int EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public EventType EventType { get; set; }

        [Column("employee_role_id")]
        public int EmployeeRoleId { get; set; }
        [ForeignKey("SystemUserRoleId")]
        public EmployeeRole EmployeeRole { get; set; }
    }
}
