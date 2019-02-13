using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class UpdatePublicHolidayViewModel : ViewModel
  {
    public int PublicHolidayId { get; set; }
    public int CountryId { get; set; }
    public DateTime Date { get; set; }
  }
}