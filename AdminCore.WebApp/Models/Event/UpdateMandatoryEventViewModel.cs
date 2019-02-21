using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class UpdateMandatoryEventViewModel : ViewModel
  {
    public int MandatoryEventId { get; set; }
    public int CountryId { get; set; }
    public DateTime Date { get; set; }
  }
}