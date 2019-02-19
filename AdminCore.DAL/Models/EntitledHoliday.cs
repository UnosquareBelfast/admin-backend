using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("entitled_holiday")]
  public class EntitledHoliday
  {
    [Key]
    [Column("entitled_holiday_id")]
    public int EntitledHolidayId { get; set; }

    [Column("country_id")]
    public int CountryId { get; set; }

    [Column("month")]
    public int? Month { get; set; }

    [Column("years_with_company")]
    public int? YearsWithCompany { get; set; }

    [Column("entitled_holidays")]
    public int EntitledHolidays { get; set; }
  }
}