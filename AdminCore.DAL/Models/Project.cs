using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("project")]
  public class Project : ISoftDeletable
  {
    [Key]
    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("project_parent_id")]
    public int ProjectParentId { get; set; }
    [ForeignKey("ProjectParentId")]
    public virtual Project ParentProject { get; set; }

    [StringLength(50)]
    [Column("project_name")]
    public string ProjectName { get; set; }

    [Column("client_id")]
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; }

    [Column("deleted")]
    public bool Deleted{ get; set; }

    public virtual ICollection<Team> Teams { get; set; }
  }
}
