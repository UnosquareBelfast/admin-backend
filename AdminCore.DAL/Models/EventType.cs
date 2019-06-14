using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("event_type")]
  public class EventType
  {
    [Key]
    [Column("event_type_id")]
    public int EventTypeId { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("system_user_role_id")]
    public virtual int SystemUserRoleId { get; set; }

    public virtual ICollection<EventTypeRequiredResponders> EventTypeRequiredResponders { get; set; }
  }
}
