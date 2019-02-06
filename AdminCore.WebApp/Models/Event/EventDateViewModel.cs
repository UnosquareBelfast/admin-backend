using System;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class EventDateViewModel : ViewModel
  {
    public int EventDateId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsHalfDay { get; set; }

    public int EventId { get; set; }
  }
}