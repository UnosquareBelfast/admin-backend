﻿using AdminCore.Constants.Enums;
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

    internal static Event BuildEvent(int eventId, int employeeId, EventStatus eventStatus, EventType eventType, int eventWorkflowId = 0)
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
        EventWorkflowId = eventWorkflowId
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
        SystemUserRoleId = (int)SystemUserRoles.User
      };
    }

    internal static EventType WorkingFromHomeEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.WorkingFromHome,
        Description = "Working From Home",
        SystemUserRoleId = (int)SystemUserRoles.User
      };
    }

    internal static EventType SickLeaveEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.Sickness,
        Description = "Sick Leave",
        SystemUserRoleId = (int)SystemUserRoles.User
      };
    }

    internal static EventType PublicHolidayEventType()
    {
      return new EventType
      {
        EventTypeId = (int)EventTypes.PublicHoliday,
        Description = "Public Holiday",
        SystemUserRoleId = (int)SystemUserRoles.SystemAdministrator
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

    internal static EventType BuildEventType(int eventTypeId, string eventTypeDescription, int systemUserRoleId)
    {
      return new EventType
      {
        EventTypeId = eventTypeId,
        Description = eventTypeDescription,
        SystemUserRoleId = systemUserRoleId
      };
    }

    internal static Employee BuildEmployee(int employeeId, int systemUserRoleId, int totalHolidays,
      ICollection<Event> events)
    {
      return new Employee
      {
        EmployeeId = employeeId,
        TotalHolidays = totalHolidays,
        Events = events,
        SystemUser = new SystemUser
        {
          SystemUserRoleId = systemUserRoleId
        }
      };
    }

    internal static Employee BuildGenericEmployee(ICollection<Event> events)
    {
      return new Employee
      {
        EmployeeId = 1,
        TotalHolidays = 40,
        Events = events,
        SystemUser = new SystemUser
        {
          SystemUserRoleId = (int)SystemUserRoles.User
        }
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

    internal static EventDateDto GenericEventDateDto(DateTime startDate = default(DateTime), DateTime endDate = default(DateTime), bool isHalfDay = false)
    {
      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = 1,
        Event = Mapper.Map<EventDto>(BuildEvent(1, 1,
          ApprovedEventStatus(), AnnualLeaveEventType())),
        IsHalfDay = isHalfDay
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
