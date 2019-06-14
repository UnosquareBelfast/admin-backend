using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("employee_status")]
  public class EmployeeStatus
  {
    [Key]
    [Column("employee_status_id")]
    public int EmployeeStatusId { get; set; }

    [StringLength(50)]
    [Column("description")]
    public string Description { get; set; }
  }
}
