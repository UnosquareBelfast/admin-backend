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

    [StringLength(50)]
    [Column("project_name")]
    public string Projectname { get; set; }

    [ForeignKey("ClientId")]
    public virtual Client Client { get; set; }

    public virtual ICollection<Team> Teams { get; set; }
  }
}
