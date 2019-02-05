using AdminCore.WebApi.Models.Base;
using System;

namespace AdminCore.WebApi.Models.Event
{
  public class CreateEventViewModel : ViewModel
  {
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsHalfDay { get; set; }
  }
}