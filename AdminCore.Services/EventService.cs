﻿using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Extensions;
using AdminCore.Services.Base;
using AutoMapper;
using Evolve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdminCore.Services
{
  public class EventService : BaseService, IEventService
  {
    private readonly IMapper _mapper;
    private readonly IDateService _dateService;

    public EventService(IDatabaseContext databaseContext, IMapper mapper, IDateService dateService)
      : base(databaseContext)
    {
      _mapper = mapper;
      _dateService = dateService;
    }

    public IList<EventDto> GetEmployeeEvents(EventTypes eventType)
    {
      return _mapper.Map<IList<EventDto>>(QueryEmployeeEvents(eventType));
    }

    public IList<EventDto> GetByDateBetween(DateTime startDate, DateTime endDate, EventTypes eventType)
    {
      var eventTypeId = (int)eventType;
      var eventsBetweenDates = DatabaseContext.EventDatesRepository.GetAsQueryable(RetrieveEventsWithinRange(startDate, endDate))
        .Where(x => x.Event.EventTypeId == eventTypeId);

      return _mapper.Map<IList<EventDto>>(eventsBetweenDates);
    }

    public IList<EventDto> GetEventsByEmployeeId(int employeeId, EventTypes eventType)
    {
      var startOfYearDate = _dateService.GetStartOfYearDate();
      var endOfYearDate = _dateService.GetEndOfYearDate();
      var eventTypeId = (int)eventType;

      var eventIds = DatabaseContext.EventDatesRepository
        .GetAsQueryable(RetrieveEventsWithinRange(startOfYearDate, endOfYearDate))
        .Where(x => x.Event.Employee.EmployeeId == employeeId).Select(x => x.EventId).ToList();

      return _mapper.Map<IList<EventDto>>(QueryEventsByEmployeeId(eventTypeId, eventIds));
    }

    public IList<EventDateDto> GetApprovedEventDatesByEmployeeAndStartAndEndDates(DateTime startDate, DateTime endDate, int employeeId)
    {
      var eventDates = DatabaseContext.EventDatesRepository.Get(x => (x.StartDate.Date >= startDate.Date
                                                                         && x.EndDate.Date <= endDate.Date
                                                                         || x.EndDate.Date == startDate.Date)
                                                                         && x.Event.EmployeeId == employeeId
                                                                         && x.Event.EventStatusId == (int)EventStatuses.Approved,
                                                              null, x => x.Event);

      return _mapper.Map<IList<EventDateDto>>(eventDates);
    }

    public EventDto GetEvent(int id)
    {
      var eventById = GetEventById(id);
      return _mapper.Map<EventDto>(eventById);
    }

    public IList<EventDto> GetEventByStatus(EventStatuses eventStatus, EventTypes eventType)
    {
      var eventStatusId = (int)eventStatus;
      var eventTypeId = (int)eventType;
      var events = DatabaseContext.EventRepository.Get(x => x.EventStatus.EventStatusId == eventStatusId
                                                            && x.EventType.EventTypeId == eventTypeId,
                                                            null,
                                                            x => x.EventDates,
                                                            x => x.Employee,
                                                            x => x.EventType,
                                                            x => x.EventStatus,
                                                            x => x.EventMessages);

      return _mapper.Map<IList<EventDto>>(events);
    }

    public IList<EventDto> GetEventByType(EventTypes eventType)
    {
      var eventTypeId = (int)eventType;
      var events = DatabaseContext.EventRepository.Get(x => x.EventType.EventTypeId == eventTypeId,
                                                            null,
                                                            x => x.EventDates,
                                                            x => x.Employee,
                                                            x => x.EventType,
                                                            x => x.EventStatus,
                                                            x => x.EventMessages);

      return _mapper.Map<IList<EventDto>>(events);
    }

    public void RejectEvent(int eventId, string message, int employeeId)
    {
      var eventToReject = GetEventById(eventId);
      if (eventToReject != null && eventToReject.EventStatusId == (int)EventStatuses.AwaitingApproval
                                && IsNotPublicHoliday(eventToReject))
      {
        eventToReject.EventStatusId = (int)EventStatuses.Rejected;
        AddEventMessageToReject(eventToReject, EventMessageTypes.Reject, message, employeeId);
        DatabaseContext.SaveChanges();
      }
      else
      {
        throw new Exception($"Event {eventId} doesn't exist or is already rejected");
      }
    }

    public void UpdateEventStatus(int eventId, EventStatuses status)
    {
      var eventToUpdate = GetEventById(eventId);
      if (eventToUpdate != null && IsNotPublicHoliday(eventToUpdate))
      {
        eventToUpdate.EventStatusId = (int)status;
        DatabaseContext.SaveChanges();
      }
    }

    public EventDto CreateEvent(EventDateDto dates, EventTypes eventTypes, int employeeId)
    {
      CheckEventTypeAdminLevel(eventTypes, employeeId);
      var newEvent = BuildNewEvent(employeeId, eventTypes);
      UpdateEventDates(dates, newEvent);
      return ValidateRemainingHolidaysAndCreate(newEvent, dates);
    }

    public EventDto CreateEvent(EventDateDto dates, EventTypes eventTypes, Employee employee)
    {
      var newEvent = BuildNewEvent(employee, eventTypes);

      UpdateEventDates(dates, newEvent);

      var insertedEvent = DatabaseContext.EventRepository.Insert(newEvent);

      return _mapper.Map<EventDto>(insertedEvent);
    }

    public void UpdateEvent(EventDateDto eventDateDto, string message, int employeeId)
    {
      var eventToUpdate = GetEventById(eventDateDto.EventId);
      if (eventToUpdate != null && IsNotPublicHoliday(eventToUpdate))
      {
        eventToUpdate.EventDates.Clear();
        UpdateEventDates(eventDateDto, eventToUpdate);
        ValidateRemainingHolidaysAndUpdate(eventToUpdate, message, employeeId);
      }
    }

    public HolidayStatsDto GetHolidayStatsForUser(int employeeId)
    {
      var holidayStatsDto = new HolidayStatsDto
      {
        ApprovedHolidays = GetHolidaysByEmployeeAndStatus(EventStatuses.Approved, employeeId),
        PendingHolidays = GetHolidaysByEmployeeAndStatus(EventStatuses.AwaitingApproval, employeeId),
        TotalHolidays = DatabaseContext.EmployeeRepository.GetSingle(x => x.EmployeeId == employeeId).TotalHolidays
      };
      holidayStatsDto.AvailableHolidays = holidayStatsDto.TotalHolidays -
                                         (holidayStatsDto.ApprovedHolidays + holidayStatsDto.PendingHolidays);
      return holidayStatsDto;
    }

    public void IsEventValid(EventDateDto eventDates, int employeeId)
    {
      if (!IsDateRangeLessThanTotalHolidaysRemaining(eventDates, employeeId))
        throw new Exception("Not enough holidays remaining.");

      if (IsHalfDay(eventDates) && !IsSameDay(_mapper.Map<EventDate>(eventDates)))
        throw new Exception("Holiday booked contains a half day whilst being more than one day.");
    }

    public void AddMandatoryEvent(DateTime date, int countryId)
    {
      if (!IsWeekend(date) && GetMandatoryEvent(date, countryId) == null)
      {
        AddMandatoryEventToDb(date, countryId);
      }
      else
      {
        throw new Exception("Date is already booked as a Mandatory Event");
      }
    }

    public void UpdateMandatoryEvent(int mandatoryEventId, DateTime date, int countryId)
    {
      var mandatoryEvent = GetMandatoryEvent(mandatoryEventId);
      if (mandatoryEvent != null && !IsWeekend(date))
      {
        UpdateMandatoryEventDetails(date, countryId, mandatoryEvent);
        UpdateMandatoryEventInDb(mandatoryEvent);
      }
      else
      {
        throw new Exception("Mandatory Event does not exist");
      }
    }

    public IList<MandatoryEventDto> GetMandatoryEvents(int countryId)
    {
      return _mapper.Map<IList<MandatoryEventDto>>(DatabaseContext.MandatoryEventRepository.Get(x => x.CountryId == countryId));
    }

    public void DeleteMandatoryEvent(int mandatoryEventId)
    {
      var mandatoryEvent = GetMandatoryEvent(mandatoryEventId);
      if (mandatoryEvent != null)
      {
        DeleteMandatoryEventInDb(mandatoryEvent);
      }
      else
      {
        throw new Exception("Mandatory Event does not exist");
      }
    }

    private static Expression<Func<EventDate, bool>> RetrieveEventsWithinRange(DateTime startDate, DateTime endDate)
    {
      return x => (startDate < x.StartDate && endDate > x.EndDate) ||
                  (startDate > x.StartDate && endDate > x.StartDate) ||
                  (startDate > x.StartDate && startDate > x.EndDate) ||
                  (startDate > x.StartDate && endDate > x.EndDate);
    }

    private double GetHolidaysByEmployeeAndStatus(EventStatuses eventStatus, int employeeId)
    {
      var annualLeaveId = (int)EventTypes.AnnualLeave;
      var publicHolidayId = (int)EventTypes.PublicHoliday;
      var eventStatusId = (int)eventStatus;
      var events = DatabaseContext.EventRepository.Get(x => x.EventStatus.EventStatusId == eventStatusId
                                                            && (x.EventType.EventTypeId == annualLeaveId || x.EventType.EventTypeId == publicHolidayId)
                                                            && x.EmployeeId == employeeId,
                                                            null,
                                                            x => x.EventDates,
                                                            x => x.Employee,
                                                            x => x.EventType,
                                                            x => x.EventStatus,
                                                            x => x.EventMessages);
      var countHolidays = CountHolidays(events);
      return countHolidays;
    }

    private double CountHolidays(IList<Event> events)
    {
      double countHolidays = 0;
      foreach (var holidayEvent in events)
      {
        countHolidays += GetDaysInEvent(holidayEvent.EventDates);
      }

      return countHolidays;
    }

    private void SplitEventIfFallsOnAWeekend(Event newEvent, DateTime originalEndDate, DateTime startDate)
    {
      var dates = startDate.Range(originalEndDate).ToList();
      foreach (var day in dates)
      {
        if (day.DayOfWeek == DayOfWeek.Saturday)
        {
          SetEndDateToPreviousDay(newEvent, startDate, day);
          var nextStartDate = day.AddDays(2);
          SplitEventIfFallsOnAWeekend(newEvent, originalEndDate, nextStartDate);
          break;
        }
      }

      if (dates.Last().Date.Day != originalEndDate.Day || dates.Count > 5 ||
          dates.First().Date.DayOfWeek == DayOfWeek.Friday && dates.Count > 1) return;
      var lastDate = new EventDate()
      {
        StartDate = startDate,
        EndDate = originalEndDate
      };
      newEvent.EventDates.Add(lastDate);
    }

    private static void SetEndDateToPreviousDay(Event newEvent, DateTime startDate, DateTime day)
    {
      newEvent.EventDates.Add(new EventDate()
      {
        StartDate = startDate,
        EndDate = day.AddDays(-1),
        IsHalfDay = false
      });
    }

    private Event GetEventById(int id)
    {
      return DatabaseContext.EventRepository.GetSingle(x => x.EventId == id,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus,
        x => x.EventMessages);
    }

    private bool IsDateRangeLessThanTotalHolidaysRemaining(EventDateDto eventDates, int employeeId)
    {
      return !(GetHolidayStatsForUser(employeeId).AvailableHolidays < ((eventDates.EndDate - eventDates.StartDate).TotalDays) + 1);
    }

    private bool IsEventDatesAlreadyBooked(EventDateDto eventDates, int employeeId)
    {
      var employeeEvents =
        GetApprovedEventDatesByEmployeeAndStartAndEndDates(eventDates.StartDate, eventDates.EndDate, employeeId);
      if (employeeEvents.Any())
      {
        throw new Exception("Holiday dates already booked.");
      }
      return false;
    }

    private bool EmployeeHasEnoughHolidays(Event newEvent)
    {
      return GetHolidayStatsForUser(newEvent.EmployeeId).AvailableHolidays >= GetDaysInEvent(newEvent.EventDates);
    }

    private static double GetDaysInEvent(ICollection<EventDate> newEventEventDates)
    {
      double totalDays = 0;
      foreach (var eventDate in newEventEventDates)
      {
        totalDays = GetDaysFromEventDate(eventDate, totalDays);
      }

      return totalDays;
    }

    private static double GetDaysFromEventDate(EventDate eventDate, double totalDays)
    {
      if (IsSameDay(eventDate) && eventDate.IsHalfDay)
      {
        totalDays += 0.5;
      }
      else if (IsSameDay(eventDate))
      {
        totalDays += 1;
      }
      else
      {
        totalDays += (eventDate.EndDate.Day - eventDate.StartDate.Day) + 1;
      }

      return totalDays;
    }

    private static bool IsSameDay(EventDate eventDate)
    {
      return eventDate.StartDate.Day == eventDate.EndDate.Day;
    }

    private bool IsHalfDay(EventDateDto dates)
    {
      return dates.IsHalfDay;
    }

    private void UpdateEventDates(EventDateDto eventDateDto, Event eventToUpdate)
    {
      if (IsHalfDay(eventDateDto))
      {
        eventToUpdate.EventDates.Add(_mapper.Map<EventDate>(eventDateDto));
      }
      else
      {
        SplitEventIfFallsOnAWeekend(eventToUpdate, eventDateDto.EndDate, eventDateDto.StartDate);
      }
    }

    private void ValidateRemainingHolidaysAndUpdate(Event eventToUpdate, string message, int employeeId)
    {
      if (EmployeeHasEnoughHolidays(eventToUpdate))
      {
        eventToUpdate = AddEventMessage(eventToUpdate, EventMessageTypes.Update, message);
        eventToUpdate.LastModified = _dateService.GetCurrentDateTime();
        eventToUpdate.EventStatusId = (int)EventStatuses.AwaitingApproval;
        UpdateEventDatesInDb(eventToUpdate);
      }
      else
      {
        throw new Exception("Not enough holidays to book");
      }
    }

    private void UpdateEventDatesInDb(Event eventToUpdate)
    {
      RemoveOldEventDates(eventToUpdate);
      InsertNewEventDates(eventToUpdate);
      DatabaseContext.SaveChanges();
    }

    private void InsertNewEventDates(Event eventToUpdate)
    {
      foreach (var eventDate in eventToUpdate.EventDates)
      {
        eventDate.EventId = eventToUpdate.EventId;
        DatabaseContext.EventDatesRepository.Insert(eventDate);
      }
    }

    private void RemoveOldEventDates(Event eventToUpdate)
    {
      var oldEventDates = DatabaseContext.EventDatesRepository.Get(x => x.EventId == eventToUpdate.EventId);
      foreach (var eventDate in oldEventDates)
      {
        DatabaseContext.EventDatesRepository.Delete(eventDate.EventDateId);
      }
    }

    private EventDto ValidateRemainingHolidaysAndCreate(Event newEvent, EventDateDto dates)
    {
      if (EmployeeHasEnoughHolidays(newEvent) && !IsEventDatesAlreadyBooked(dates, newEvent.EmployeeId))
      {
        var insertedEvent = DatabaseContext.EventRepository.Insert(newEvent);
        DatabaseContext.SaveChanges();
        return _mapper.Map<EventDto>(insertedEvent);
      }

      throw new Exception("Not enough holidays to book");
    }

    private Event BuildNewEvent(int employeeId, EventTypes eventTypes)
    {
      var eventStatusId = AutoApproveEventsNotNeedingAdminApproval(eventTypes);

      var newEvent = new Event
      {
        DateCreated = DateTime.Now,
        EmployeeId = employeeId,
        EventStatusId = eventStatusId,
        EventTypeId = (int)eventTypes,
        EventDates = new List<EventDate>(),
        LastModified = _dateService.GetCurrentDateTime()
      };
      return newEvent;
    }

    private Event BuildNewEvent(Employee employee, EventTypes eventTypes)
    {
      var newEvent = new Event
      {
        DateCreated = DateTime.Now,
        Employee = employee,
        EventStatusId = (int)EventStatuses.Approved,
        EventTypeId = (int)eventTypes,
        EventDates = new List<EventDate>(),
        LastModified = _dateService.GetCurrentDateTime()
      };
      return newEvent;
    }

    private Event AddEventMessage(Event eventToUpdate, EventMessageTypes eventMessageTypes, string message)
    {
      if (message.IsNullOrWhiteSpace())
      {
        return eventToUpdate;
      }

      if (eventToUpdate.EventMessages == null)
      {
        eventToUpdate.EventMessages = new List<EventMessage>();
      }

      eventToUpdate.EventMessages.Add(AddEventMessageToEvent(eventToUpdate, eventMessageTypes, message));
      return eventToUpdate;
    }

    private EventMessage AddEventMessageToEvent(Event eventToAddMessageTo, EventMessageTypes eventMessageType, string message)
    {
      EventMessage eventMessage = new EventMessage
      {
        EventId = eventToAddMessageTo.EventId,
        EventMessageTypeId = (int)eventMessageType,
        EmployeeId = eventToAddMessageTo.EmployeeId,
        LastModified = _dateService.GetCurrentDateTime(),
        Message = message,
      };

      return eventMessage;
    }

    private Event AddEventMessageToReject(Event eventToUpdate, EventMessageTypes eventMessageTypes, string message, int employeeId)
    {
      if (message.IsNullOrWhiteSpace())
      {
        return eventToUpdate;
      }

      if (eventToUpdate.EventMessages == null)
      {
        eventToUpdate.EventMessages = new List<EventMessage>();
      }

      eventToUpdate.EventMessages.Add(AddEventMessageToRejectEvent(eventToUpdate, eventMessageTypes, message, employeeId));
      return eventToUpdate;
    }

    private EventMessage AddEventMessageToRejectEvent(Event eventToAddMessageTo, EventMessageTypes eventMessageType, string message, int employeeId)
    {
      EventMessage eventMessage = new EventMessage
      {
        EventId = eventToAddMessageTo.EventId,
        EventMessageTypeId = (int)eventMessageType,
        EmployeeId = employeeId,
        LastModified = _dateService.GetCurrentDateTime(),
        Message = message,
      };

      return eventMessage;
    }

    private IList<Event> QueryHolidays()
    {
      var annualLeaveId = (int)EventTypes.AnnualLeave;
      var publicHolidayId = (int)EventTypes.PublicHoliday;
      var events = DatabaseContext.EventRepository.Get(x =>
          (x.EventType.EventTypeId == annualLeaveId || x.EventType.EventTypeId == publicHolidayId),
        null,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus,
        x => x.EventMessages);
      return events;
    }

    private IList<Event> QueryOtherEvents(int eventTypeId)
    {
      var events = DatabaseContext.EventRepository.Get(x =>
          x.EventType.EventTypeId == eventTypeId,
        null,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus,
        x => x.EventMessages);
      return events;
    }

    private IList<Event> QueryEmployeeEvents(EventTypes eventType)
    {
      IList<Event> events;
      var eventTypeId = (int)eventType;
      if (eventTypeId == (int)EventTypes.AnnualLeave)
      {
        events = QueryHolidays();
      }
      else
      {
        events = QueryOtherEvents(eventTypeId);
      }

      return events;
    }

    private IList<Event> QueryEventsByEmployeeId(int eventTypeId, List<int> eventIds)
    {
      IList<Event> events;
      if (eventTypeId == (int)EventTypes.AnnualLeave)
      {
        events = QueryEmployeeHolidayEvents(eventIds);
      }
      else
      {
        events = QueryOtherEmployeeEvents(eventIds, eventTypeId);
      }

      return events;
    }

    private IList<Event> QueryEmployeeHolidayEvents(List<int> eventIds)
    {
      var annualLeaveId = (int)EventTypes.AnnualLeave;
      var publicHolidayId = (int)EventTypes.PublicHoliday;
      var events = DatabaseContext.EventRepository.Get(x => eventIds.Contains(x.EventId)
                                                            && (x.EventType.EventTypeId == annualLeaveId
                                                                || x.EventType.EventTypeId == publicHolidayId),
        null,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus,
        x => x.EventMessages);
      return events;
    }

    private IList<Event> QueryOtherEmployeeEvents(List<int> eventIds, int eventTypeId)
    {
      var events = DatabaseContext.EventRepository.Get(x => eventIds.Contains(x.EventId)
                                                            && x.EventTypeId == eventTypeId,
        null,
        x => x.EventDates,
        x => x.Employee,
        x => x.EventType,
        x => x.EventStatus,
        x => x.EventMessages);
      return events;
    }

    private static bool IsNotPublicHoliday(Event eventToUpdate)
    {
      return eventToUpdate.EventTypeId != (int)EventTypes.PublicHoliday;
    }

    private static int AutoApproveEventsNotNeedingAdminApproval(EventTypes eventTypes)
    {
      var eventStatusId = (int)EventStatuses.AwaitingApproval;
      if (eventTypes == EventTypes.WorkingFromHome || eventTypes == EventTypes.Sickness)
      {
        eventStatusId = (int)EventStatuses.Approved;
      }

      return eventStatusId;
    }

    private void CheckEventTypeAdminLevel(EventTypes eventTypes, int employeeId)
    {
      var eventTypeId = (int)eventTypes;
      var employeeRoleLevelRequired = DatabaseContext.EventTypeRepository.GetAsQueryable(x => x.EventTypeId == eventTypeId)
                                                                           .Select(x => x.EmployeeRoleId).FirstOrDefault();
      var employeeRole = DatabaseContext.EmployeeRepository.GetAsQueryable(x => x.EmployeeId == employeeId)
                                                                           .Select(x => x.EmployeeRoleId).FirstOrDefault();
      if (UserDoesNotHaveCorrectPrivileges(employeeRoleLevelRequired, employeeRole))
      {
        throw new Exception("User does not have the correct privileges to book this type of event.");
      }
    }

    private static bool UserDoesNotHaveCorrectPrivileges(int employeeRoleLevelRequired, int employeeLevel)
    {
      return employeeRoleLevelRequired == (int)EmployeeRoles.SystemAdministrator && employeeLevel == (int)EmployeeRoles.User;
    }

    private void AddMandatoryEventToDb(DateTime date, int countryId)
    {
      var mandatoryEvent = new MandatoryEvent { MandatoryEventDate = date, CountryId = countryId };
      DatabaseContext.MandatoryEventRepository.Insert(mandatoryEvent);
      DatabaseContext.SaveChanges();
    }

    private static bool IsWeekend(DateTime date)
    {
      return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    private MandatoryEvent GetMandatoryEvent(int mandatoryEventId)
    {
      return DatabaseContext.MandatoryEventRepository.GetSingle(x =>
        x.MandatoryEventId == mandatoryEventId);
    }

    private MandatoryEvent GetMandatoryEvent(DateTime date, int countryId)
    {
      return DatabaseContext.MandatoryEventRepository.GetSingle(x =>
        x.MandatoryEventDate.Date == date.Date && x.CountryId == countryId);
    }

    private void UpdateMandatoryEventInDb(MandatoryEvent mandatoryEvent)
    {
      DatabaseContext.MandatoryEventRepository.Update(mandatoryEvent);
      DatabaseContext.SaveChanges();
    }

    private static void UpdateMandatoryEventDetails(DateTime date, int countryId, MandatoryEvent mandatoryEvent)
    {
      mandatoryEvent.MandatoryEventDate = date;
      mandatoryEvent.CountryId = countryId;
    }

    private void DeleteMandatoryEventInDb(MandatoryEvent mandatoryEvent)
    {
      DatabaseContext.MandatoryEventRepository.Delete(mandatoryEvent);
      DatabaseContext.SaveChanges();
    }
  }
}