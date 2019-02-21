using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
  [Table("public_holiday")]
  public class MandatoryEvent
  {
    [Key]
    [Column("mandatory_event_id")]
    public int MandatoryEventId { get; set; }

    [Column("country_id")]
    public int CountryId { get; set; }

    [Column("mandatory_event_date")]
    public DateTime MandatoryEventDate { get; set; }
  }
}