using AdminCore.DTOs.Event;
using AdminCore.DTOs.EventMessage;
using System;
using System.Collections.Generic;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Dashboard
{
  public class DashboardEventViewModel : ViewModel
  {
    public int EventId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime DateCreated { get; set; }

    public int EventStatusId { get; set; }

    public int EventTypeId { get; set; }

    public ICollection<EventMessageDto> EventMessages { get; set; }

    public string LatestMessage { get; set; }

    public ICollection<EventDateDto> EventDates { get; set; }

    public DateTime LastModified { get; set; }
  }
}