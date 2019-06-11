using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("client")]
  public class Client : ISoftDeletable
  {
    [Key]
    [Column("client_id")]
    public int ClientId { get; set; }

    [Column("system_user_id")]
    public int SystemUserId { get; set; }

    [StringLength(50)]
    [Column("client_name")]
    public string ClientName { get; set; }

    [ForeignKey("SystemUserId")]
    public virtual SystemUser SystemUser { get; set; }

    public virtual ICollection<Project> Projects { get; set; }
  }
}
