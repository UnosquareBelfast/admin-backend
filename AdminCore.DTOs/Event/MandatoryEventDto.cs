using System;

namespace AdminCore.DTOs.Event
{
  public class MandatoryEventDto
  {
    public int MandatoryEventId { get; set; }

    public int CountryId { get; set; }

    public DateTime MandatoryEventDate { get; set; }
  }
}