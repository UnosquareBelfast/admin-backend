using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("public_holiday")]
  public class PublicHoliday
  {
    [Key]
    [Column("public_holiday_id")]
    public int PublicHolidayId { get; set; }

    [Column("country_id")]
    public int CountryId { get; set; }

    [Column("public_holiday_date")]
    public DateTime PublicHolidayDate { get; set; }
  }
}