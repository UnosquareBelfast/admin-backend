using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("approval_status")]
  public class ApprovalStatus : ISoftDeletable
  {
    [Key]
    [Column("approval_status_id")]
    public int ApprovalStatusId { get; set; }

    [Column("description")]
    public string Description { get; set; }
  }
}