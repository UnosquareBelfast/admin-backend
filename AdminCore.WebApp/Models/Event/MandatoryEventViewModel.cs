using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class MandatoryEventViewModel : ViewModel
  {
    public int MandatoryEventId { get; set; }

    public int CountryId { get; set; }

    public DateTime MandatoryEventDate { get; set; }
  }
}