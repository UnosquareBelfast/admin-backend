using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("northern_irish_holiday")]
  public class NorthernIrishHoliday
  {
    [Key]
    [Column("northern_irish_holiday_id")]
    public int NorthernIrishHolidayId { get; set; }

    [Column("month")]
    public int Month { get; set; }

    [Column("entitled_holidays")]
    public int EntitledHolidays { get; set; }
  }
}