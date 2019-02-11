using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("mexican_holiday")]
  public class MexicanHoliday
  {
    [Key]
    [Column("mexican_holiday_id")]
    public int MexicanHolidayId { get; set; }

    [Column("years_with_company")]
    public int YearsWithCompany { get; set; }

    [Column("entitled_holidays")]
    public int EntitledHolidays { get; set; }
  }
}