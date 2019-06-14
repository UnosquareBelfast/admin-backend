using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("employee_role")]
  public class SystemUserRole
  {
    [Key]
    [Column("system_user_role_id")]
    public int SystemUserRoleId { get; set; }

    [StringLength(50)]
    [Column("description")]
    public string Description { get; set; }

    public virtual ICollection<EventTypeRequiredResponders> EventTypeRequiredResponders { get; set; }
  }
}
