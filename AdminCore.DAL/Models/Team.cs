using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("team")]
  public class Team : ISoftDeletable
  {
    [Key]
    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("team_name")]
    public string TeamName { get; set; }

    [Column("contact_email")]
    public string ContactEmail { get; set; }

    [Column("contact_name")]
    public string ContactName { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Project project { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; }
  }
}
