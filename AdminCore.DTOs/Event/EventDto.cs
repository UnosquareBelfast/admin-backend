using AdminCore.DTOs.Employee;
using AdminCore.DTOs.EventMessage;
using System;
using System.Collections.Generic;
using AdminCore.DTOs.EventWorkflow;

namespace AdminCore.DTOs.Event
{
  public class EventDto
  {
    public int EventId { get; set; }

    public int EmployeeId { get; set; }

    public EmployeeDto Employee { get; set; }

    public DateTime DateCreated { get; set; }

    public int EventStatusId { get; set; }

    public EventStatusDto EventStatus { get; set; }

    public int EventWorkflowId { get; set; }
    public virtual EventWorkflowDto EventWorkflow { get; set; }
    
    public int EventTypeId { get; set; }

    public EventTypeDto EventType { get; set; }

    public ICollection<EventMessageDto> EventMessages { get; set; }

    public string LatestMessage { get; set; }

    public ICollection<EventDateDto> EventDates { get; set; }

    public DateTime LastModified { get; set; }
  }
}