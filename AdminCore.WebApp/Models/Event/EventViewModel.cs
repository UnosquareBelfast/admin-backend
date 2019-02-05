using AdminCore.WebApi.Models.Employee;
using AdminCore.WebApi.Models.EventMessage;
using System;
using System.Collections.Generic;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class EventViewModel : ViewModel
  {
    public int EventId { get; set; }

    public ICollection<EventDateViewModel> EventDates { get; set; }

    public EmployeeViewModel Employee { get; set; }

    public EventStatusViewModel EventStatus { get; set; }

    public EventTypeViewModel EventType { get; set; }

    public bool IsHalfDay { get; set; }

    public ICollection<EventMessageViewModel> EventMessages { get; set; }

    public string LatestMessage { get; set; }

    public DateTime LastModified { get; set; }

    public DateTime DateCreated { get; set; }
  }
}