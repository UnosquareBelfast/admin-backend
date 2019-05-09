using AdminCore.Constants.Enums;
using AdminCore.DTOs.Event;
using System;
using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
  public interface IEventService
  {
    IList<EventDto> GetEmployeeEvents(EventTypes eventType);

    IList<EventDto> GetByDateBetween(DateTime startDate, DateTime endDate, EventTypes eventType);

    IList<EventDto> GetEventsByEmployeeId(int employeeId, EventTypes eventType);

    IList<EventDateDto> GetApprovedEventDatesByEmployeeAndStartAndEndDates(DateTime startDate, DateTime endDate, int employeeId);

    EventDto GetEvent(int id);

    IList<EventDto> GetEventByStatus(EventStatuses eventStatus, EventTypes eventType);

    IList<EventDto> GetEventByType(EventTypes eventType);

    HolidayStatsDto GetHolidayStatsForUser(int employeeId);

    EventDto CreateEvent(EventDateDto dates, EventTypes eventTypes, int employeeId, int eventWorkflowId);

    void UpdateEvent(EventDateDto eventDateDto, string message, int employeeId);

    void UpdateEventStatus(int eventId, EventStatuses status);

    void AddRejectMessageToEvent(int eventId, string message, int employeeId);

    void IsEventValid(EventDateDto eventDates, int employeeId);

    void AddMandatoryEvent(DateTime date, int countryId);

    void DeleteMandatoryEvent(int mandatoryEventId);

    IList<MandatoryEventDto> GetMandatoryEvents(int countryId);

    void UpdateMandatoryEvent(int mandatoryEventId, DateTime date, int countryId);
  }
}