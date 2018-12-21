﻿using AdminCore.Constants.Enums;
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

    IList<EventDateDto> GetEventDatesByEmployeeAndStartAndEndDates(DateTime startDate, DateTime endDate);

    EventDto GetEvent(int id);

    IList<EventDto> GetEventByStatus(EventStatuses eventStatus, EventTypes eventType);

    IList<EventDto> GetEventByType(EventTypes eventType);

    HolidayStatsDto GetHolidayStatsForUser();

    EventDto CreateEvent(EventDateDto dates, EventTypes eventTypes);

    void UpdateEvent(EventDateDto eventDateDto);

    void UpdateEventStatus(int eventId, EventStatuses status);

    void RejectEvent(int eventId, string message);

    void IsHolidayValid(EventDateDto eventDates, bool modelIsHalfDay);
  }
}