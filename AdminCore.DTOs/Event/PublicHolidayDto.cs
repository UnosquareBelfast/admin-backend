using System;

namespace AdminCore.DTOs.Event
{
  public class PublicHolidayDto
  {
    public int PublicHolidayId { get; set; }

    public int CountryId { get; set; }

    public DateTime PublicHolidayDate { get; set; }
  }
}