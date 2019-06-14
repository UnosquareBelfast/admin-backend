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

        [Column("system_user_role_id")]
        public int SystemUserRoleId { get; set; }
        [ForeignKey("SystemUserRoleId")]
        public SystemUserRole SystemUserRole { get; set; }
    }
}
