using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class CreateMandatoryEventViewModel : ViewModel
  {
    public int CountryId { get; set; }
    public DateTime Date { get; set; }
  }
}