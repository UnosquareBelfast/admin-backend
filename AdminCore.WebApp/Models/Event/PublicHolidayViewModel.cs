using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class PublicHolidayViewModel : ViewModel
  {
    public int PublicHolidayId { get; set; }

    public int CountryId { get; set; }

    public DateTime PublicHolidayDate { get; set; }
  }
}