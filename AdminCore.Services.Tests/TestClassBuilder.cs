using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Services.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace AdminCore.Services.Tests
{
  public sealed class TestClassBuilder
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));

    internal static Event BuildEvent(int eventId, int employeeId, EventStatus eventStatus, EventType eventType)
    {
      return new Event
      {
        EventId = eventId,
        DateCreated = DateTime.Now,
        EmployeeId = employeeId,
        EventStatus = eventStatus,
        EventStatusId = eventStatus.EventStatusId,
        EventType = eventType,
        EventTypeId = eventType.EventTypeId,
      };
    }

    internal static Event BuildEvent(int eventId, int employeeId, EventStatus eventStatus, EventType eventType,
      IList<EventDate> eventDates)
    {
      return new Event
      {
        EventId = eventId,
        DateCreated = DateTime.Now,
        EmployeeId = employeeId,
        EventStatus = eventStatus,
        EventStatusId = eventStatus.EventStatusId,
        EventType = eventType,
        EventTypeId = eventType.EventTypeId,
        EventDates = eventDates
      };
    }

    internal static EventType AnnualLeaveEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.AnnualLeave,
        Description = "Annual Leave",
        EmployeeRoleId = (int)EmployeeRoles.User
      };
    }

    internal static EventType SickLeaveEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.Sickness,
        Description = "Sick Leave",
        EmployeeRoleId = (int)EmployeeRoles.User
      };
    }

    internal static EventType PublicHolidayEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.PublicHoliday,
        Description = "Public Holiday",
        EmployeeRoleId = (int)EmployeeRoles.SystemAdministrator
      };
    }

    internal static EventStatus ApprovedEventStatus()
    {
      return new EventStatus
      {
        EventStatusId = (int)EventStatuses.Approved,
        Description = "Approved"
      };
    }

    internal static EventStatus AwaitingApprovalEventStatus()
    {
      return new EventStatus
      {
        EventStatusId = (int)EventStatuses.AwaitingApproval,
        Description = "Awaiting Approval"
      };
    }

    internal static EventStatus RejectedEventStatus()
    {
      return new EventStatus
      {
        EventStatusId = (int)EventStatuses.Rejected,
        Description = "Rejected"
      };
    }

    internal static EventType BuildEventType(int eventTypeId, string eventTypeDescription, int employeeRoleId)
    {
      return new EventType
      {
        EventTypeId = eventTypeId,
        Description = eventTypeDescription,
        EmployeeRoleId = employeeRoleId
      };
    }

    internal static Employee BuildEmployee(int employeeId, int employeeRoleId, int totalHolidays,
      ICollection<Event> events)
    {
      return new Employee
      {
        EmployeeId = employeeId,
        EmployeeRoleId = employeeRoleId,
        TotalHolidays = totalHolidays,
        Events = events
      };
    }

    internal static Employee BuildGenericEmployee(ICollection<Event> events)
    {
      return new Employee
      {
        EmployeeId = 1,
        EmployeeRoleId = (int)EmployeeRoles.User,
        TotalHolidays = 40,
        Events = events
      };
    }

    internal static EventStatus BuildEventStatus(int eventStatusId, string eventStatusDescription)
    {
      return new EventStatus
      {
        EventStatusId = eventStatusId,
        Description = eventStatusDescription
      };
    }

    internal static EventDateDto GenericEventDateDto(DateTime startDate = default(DateTime), DateTime endDate = default(DateTime))
    {
      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = 1,
        Event = Mapper.Map<EventDto>(BuildEvent(1, 1,
          ApprovedEventStatus(), AnnualLeaveEventType())),
        IsHalfDay = false
      };
      return eventDateDto;
    }

    internal static EventDateDto BuildEventDateDto(DateTime startDate, DateTime endDate,
      int eventId, int employeeId, EventStatus eventStatus, EventType eventType)
    {
      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(BuildEvent(1, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      return eventDateDto;
    }
  }
}
