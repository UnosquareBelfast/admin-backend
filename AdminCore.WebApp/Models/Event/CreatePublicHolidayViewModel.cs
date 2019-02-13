using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class CreatePublicHolidayViewModel : ViewModel
  {
    public DateTime Date { get; set; }
    public int CountryId { get; set; }
  }
}