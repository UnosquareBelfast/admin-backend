﻿using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Services.Mappings;
using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class EventServiceTests : BaseMockedDatabaseSetUp
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    public EventServiceTests()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, new List<Event>());
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, new List<EventDate>());

      var eventService = GetEventService(databaseContext);

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneDayWhileExactEventAlreadyExistsWithAwaitingApprovalStatus_DoesNotInsertsNewEventIntoDb()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, new List<Event>());
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId));

      // Assert
      Assert.Equal("Holiday dates already booked.", ex.Message);
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneHalfDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 11, 05);
      var endDate = new DateTime(2018, 11, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = true
      };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, new List<Event>());
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, new List<EventDate>());

      var eventService = GetEventService(databaseContext);

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneHalfDayWhileExactEventAlreadyExistsWithAwaitingApprovalStatus_DoesNotInsertsNewEventIntoDb()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 11, 05);
      var endDate = new DateTime(2018, 11, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = true
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, new List<Event>());
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId));

      // Assert
      Assert.Equal("Holiday dates already booked.", ex.Message);
    }

    [Fact]
    public void CreateEvent_WhenEventDatesAlreadyBooked_ThrowsAlreadyBookedException()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 10, 05);
      var endDate = new DateTime(2018, 10, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      databaseContext.EventRepository.Insert(newEvent);
      var ex = Assert.Throws<Exception>(() =>
        eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId));

      // Assert
      Assert.Equal("Holiday dates already booked.", ex.Message);
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfMultipleDays_SuccessfullyInsertsNewEventIntoDb()
    {
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 09, 03);
      var endDate = new DateTime(2018, 09, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(new EventDateDto()) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_WithSickLeaveEventType_SuccessfullyInsertsNewEventIntoDbAndIsAutoApproved()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.SickLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, new List<Event>());
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, new List<EventDate>());

      var eventService = GetEventService(databaseContext);

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.Sickness, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_WithoutEnoughAvailableHolidays_ThrowsNotEnoughHolidaysException()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2019, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId));

      // Assert
      Assert.Equal("Not enough holidays to book", ex.Message);
    }

    [Fact]
    public void CreateEvent_WithoutAdequateUserRole_ThrowsIncorrectPrivilegesException()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.BuildEventType((int)EventTypes.PaternityLeave,
        "Paternal Leave", (int)EmployeeRoles.SystemAdministrator);
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.CreateEvent(eventDateDto, EventTypes.PaternityLeave, employeeId));

      // Assert
      Assert.Equal("User does not have the correct privileges to book this type of event.", ex.Message);
    }

    [Fact]
    public void CreateEvent_WithValidEmployee_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employee);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void GetEmployeeEvents_WithAnnualLeaveEventType_ReturnsListOfEmployeeAnnualLeaveEvents()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.AwaitingApprovalEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      //Act
      var annualLeaveEventsList = eventService.GetEmployeeEvents(EventTypes.AnnualLeave);

      // Assert
      Assert.Equal(1, annualLeaveEventsList.Count);
    }

    [Fact]
    public void GetEmployeeEvents_WithSickLeaveEventType_ReturnsListOfEmployeeSickLeaveEvents()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.SickLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var sickLeaveEventsList = eventService.GetEmployeeEvents(EventTypes.Sickness);

      // Assert
      Assert.Equal(1, sickLeaveEventsList.Count);
    }

    [Fact]
    public void GetByDateBetween_WithValidDateAndAnnualLeaveEventType_ReturnsListOfAnnualLeaveEventsBetweenDatesGiven()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 03);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, null);
      var employeeList = new List<Employee> { employee };

      var eventDateDto = new EventDateDto
      {
        StartDate = startDate,
        EndDate = endDate,
        EventId = eventId,
        Event = Mapper.Map<EventDto>(TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType)),
        IsHalfDay = false
      };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);

      var eventService = GetEventService(databaseContext);

      // Act
      var annualLeaveEventsByDateList = eventService.GetByDateBetween(startDate.AddDays(-1), endDate.AddDays(1), EventTypes.AnnualLeave);

      // Assert
      Assert.Equal(1, annualLeaveEventsByDateList.Count);
    }

    [Fact]
    public void
      GetEventsByEmployeeId_WithValidEmployeeAndAnnualLeaveEventType_ReturnsListOfAnnualLeaveEventsForThatEmployee()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 03);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var eventDateDto = TestClassBuilder.BuildEventDateDto(startDate, endDate,
        eventId, employeeId, eventStatus, eventType);

      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var employee = TestClassBuilder.BuildEmployee(employeeId,
        (int)EmployeeRoles.User, 40, events);
      var employeeList = new List<Employee> { employee };

      var eventWithEmployee = newEvent;
      eventWithEmployee.Employee = employee;
      var eventWithEmployeeList = new List<Event>
      {
        eventWithEmployee
      };

      var eventDateWithEventAndEmployeeList = Mapper.Map<EventDate>(eventDateDto);
      eventDateWithEventAndEmployeeList.Event = eventWithEmployee;
      var eventDatesWithEventAndEmployeeList = new List<EventDate> { eventDateWithEventAndEmployeeList };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, eventWithEmployeeList);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesWithEventAndEmployeeList);

      var eventService = GetEventService(databaseContext);

      // Act
      var eventsByEmployeeId = eventService.GetEventsByEmployeeId(employeeId, EventTypes.AnnualLeave);

      // Assert
      Assert.Equal(1, eventsByEmployeeId.Count);
    }

    [Fact]
    public void GetEventsByEmployeeId_WithValidEmployeeAndSickLeaveEventType_ReturnsListOfEventsForThatEmployee()
    {
      // Arrange
      const int employeeId = 1;
      const int eventId = 1;
      var startDate = new DateTime(2018, 12, 05);
      var endDate = new DateTime(2018, 12, 05);

      var eventType = TestClassBuilder.SickLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var eventDateDto = TestClassBuilder.BuildEventDateDto(startDate, endDate, eventId, employeeId, eventStatus, eventType);
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var employee = TestClassBuilder.BuildGenericEmployee(events);
      var employeeList = new List<Employee> { employee };

      var eventWithEmployee = newEvent;
      eventWithEmployee.Employee = employee;
      var eventWithEmployeeList = new List<Event>
      {
        eventWithEmployee
      };

      var eventDateWithEventAndEmployeeList = Mapper.Map<EventDate>(eventDateDto);
      eventDateWithEventAndEmployeeList.Event = eventWithEmployee;
      var eventDatesWithEventAndEmployeeList = new List<EventDate> { eventDateWithEventAndEmployeeList };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, eventWithEmployeeList);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesWithEventAndEmployeeList);

      var eventService = GetEventService(databaseContext);

      // Act
      var eventsByEmployeeId = eventService.GetEventsByEmployeeId(employeeId, EventTypes.Sickness);

      // Assert
      Assert.Equal(1, eventsByEmployeeId.Count);
    }

    public static TheoryData<Tuple<DateTime, DateTime>> EventDates = new TheoryData<Tuple<DateTime, DateTime>>
    {
      new Tuple<DateTime, DateTime>(new DateTime(2019, 5, 13), new DateTime(2019, 5, 16)),
      new Tuple<DateTime, DateTime>(new DateTime(2019, 5, 14), new DateTime(2019, 5, 16)),
      new Tuple<DateTime, DateTime>(new DateTime(2019, 5, 14), new DateTime(2019, 5, 17)),
      new Tuple<DateTime, DateTime>(new DateTime(2019, 5, 13), new DateTime(2019, 5, 17)),
      new Tuple<DateTime, DateTime>(new DateTime(2019, 5, 10), new DateTime(2019, 5, 20))
    };

    [Theory]
    [MemberData(nameof(EventDates))]
    public void isEventAlreadyBooked_WithAlreadyBookedEventDates_ShouldReturnAlreadyBookedEvents(Tuple<DateTime, DateTime> eventDates)
    {
      var (startDate, endDate) = eventDates;

      // Arrange
      const int employeeId = 1;
      const int eventId = 1;

      var eventType =
        TestClassBuilder.AnnualLeaveEventType();
      var eventTypesList = new List<EventType> { eventType };
      var eventStatus = TestClassBuilder.ApprovedEventStatus();

      var eventDateDto = TestClassBuilder.BuildEventDateDto(
        new DateTime(2019, 5, 13),
        new DateTime(2019, 5, 17),
        eventId,
        employeeId,
        eventStatus,
        eventType);

      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(eventDateDto) };

      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, eventStatus, eventType, eventDatesList);
      var events = new List<Event> { newEvent };

      var employee = TestClassBuilder.BuildGenericEmployee(events);
      var employeeList = new List<Employee> { employee };

      var eventWithEmployee = newEvent;
      eventWithEmployee.Employee = employee;
      var eventWithEmployeeList = new List<Event>
      {
        eventWithEmployee
      };

      var eventDateWithEventAndEmployeeList = Mapper.Map<EventDate>(eventDateDto);
      eventDateWithEventAndEmployeeList.Event = eventWithEmployee;
      var eventDatesWithEventAndEmployeeList = new List<EventDate> { eventDateWithEventAndEmployeeList };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, eventWithEmployeeList);
      databaseContext = SetUpEventTypeRepository(databaseContext, eventTypesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesWithEventAndEmployeeList);

      var eventService = GetEventService(databaseContext);

      // Act
      var isEventAlreadyBooked = eventService.IsEventAlreadyBooked(startDate, endDate, employeeId, EventStatuses.Approved);

      // Assert
      Assert.True(isEventAlreadyBooked);
    }

    [Fact]
    public void GetEventById_WithExistingEventId_ReturnsEvent()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      var returnedEvent = eventService.GetEvent(eventId);

      // Assert
      Assert.Equal(eventId, returnedEvent.EventId);
    }

    [Fact]
    public void GetEventById_WithNonExistingEventId_ReturnsNull()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      var returnedEvent = eventService.GetEvent(2);

      // Assert
      Assert.Null(returnedEvent);
    }

    [Fact]
    public void GetEventByStatus_WithApprovedEventStatus_ReturnsAllEventsOfApprovedStatusType()
    {
      // Arrange
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var approvedSickEvent = TestClassBuilder.BuildEvent(1, 1, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.SickLeaveEventType(), eventDatesList);
      var approvedAnnualLeaveEvent = TestClassBuilder.BuildEvent(2, 2, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { approvedSickEvent, approvedAnnualLeaveEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      var returnedEvents = eventService.GetEventByStatus(EventStatuses.Approved, EventTypes.AnnualLeave);

      // Assert
      Assert.Equal(1, returnedEvents.Count);
    }

    [Fact]
    public void GetEventByType_WithAnnualLeaveEventType_ReturnsListOfAllAnnualLeaveEvents()
    {
      // Arrange
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var annualLeaveEvent = TestClassBuilder.BuildEvent(1, 1, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { annualLeaveEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      var returnedEvents = eventService.GetEventByType(EventTypes.AnnualLeave);

      // Assert
      Assert.Equal(1, returnedEvents.Count);
    }

    [Fact]
    public void RejectEvent_WithValidEvent_RejectsTheEventOfEventIdProvided()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.AwaitingApprovalEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      var message = "Rejected";
      var employeeIdRejecting = 2;

      // Act
      eventService.RejectEvent(eventId, message, employeeIdRejecting);

      // Assert
      databaseContext.Received().SaveChanges();
    }

    [Fact]
    public void RejectEvent_WithValidEventAndNoRejectMessage_RejectsTheEventOfEventIdProvided()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.AwaitingApprovalEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      var employeeIdRejecting = 2;

      // Act
      eventService.RejectEvent(eventId, null, employeeIdRejecting);

      // Assert
      databaseContext.Received().SaveChanges();
    }

    [Fact]
    public void RejectEvent_WhenCalledWithEventAlreadyRejected_ThrowsException()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.RejectedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      var message = "Rejected";
      var employeeIdRejecting = 2;

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.RejectEvent(eventId, message, employeeIdRejecting));

      // Assert
      Assert.Equal($"Event {eventId} doesn't exist or is already rejected", ex.Message);
    }

    [Fact]
    public void RejectEvent_WhenCalledWithEventIdNotExisting_ThrowsException()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.RejectedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      var nonExistentEventId = 9004;
      var message = "Rejected";
      var employeeIdRejecting = 2;

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.RejectEvent(nonExistentEventId, message, employeeIdRejecting));

      // Assert
      Assert.Equal($"Event {nonExistentEventId} doesn't exist or is already rejected", ex.Message);
    }

    [Fact]
    public void UpdateEventStatus_WithUpdatedStatusBeingCancelled_UpdatesEventStatusToCancelled()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      eventService.UpdateEventStatus(eventId, EventStatuses.Cancelled);

      // Assert
      Assert.Equal((int)EventStatuses.Cancelled, eventService.GetEvent(eventId).EventStatusId);
    }

    [Fact]
    public void UpdateEventStatus_WhenCalledOnEventThatCannotHaveEventStatusUpdated_DoesNotUpdatesEventStatusForPublicHoliday()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.PublicHolidayEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      var eventService = GetEventService(databaseContext);

      // Act
      eventService.UpdateEventStatus(eventId, EventStatuses.Cancelled);

      // Assert
      Assert.NotEqual((int)EventStatuses.Cancelled, eventService.GetEvent(eventId).EventStatusId);
    }

    [Fact]
    public void UpdateEvent_WhenCalled_UpdatesTheEventWithNewDates()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var message = "Update Message";
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06)
      };

      // Act
      eventService.UpdateEvent(eventDateDto, message, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Delete(Arg.Any<Event>());
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }
    
    [Fact]
    public void UpdateEvent_WhenCalledToUpdateEventWithIdenticalStartAndEndDatesAndHalfDayValue_DoesNotUpdateTheEventWithNewRangeOfDates()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var message = "Update Message";
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto(
        new DateTime(2018, 12, 04),
        new DateTime(2018, 12, 06),
        true)) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06)
      });
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06),
        IsHalfDay = true
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, "test message", employeeId));

      // Assert
      Assert.Equal("Proposed changes are identical to attributes of the current event", ex.Message);
    }

    [Fact]
    public void UpdateEvent_WhenCalledWithNullMessage_DoesNotUpdateTheEventWithNewDates()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06)
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, null, employeeId));

      // Assert
      Assert.Equal("Cannot update event, criteria not met", ex.Message);
    }

    [Fact]
    public void UpdateEvent_WhenCalledOnEventThatCannotBeUpdated_DoesNotUpdateThePublicHolidayWithNewDates()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.PublicHolidayEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06)
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, "test message", employeeId));

      // Assert
      Assert.Equal("Cannot update event, criteria not met", ex.Message);
    }

    [Fact]
    public void UpdateEvent_WhenNotEnoughHolidays_DoesNotUpdateEvent()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDatesList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2019, 12, 06)
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, "test message", employeeId));

      // Assert
      Assert.Equal("Not enough holidays to book", ex.Message);
    }

    [Fact]
    public void UpdateEvent_UpdateFromHalfDayToFullDayEvent_UpdatesEventToFullDay()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var message = "Update Message";
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto(
        new DateTime(2018, 12, 04),
         new DateTime(2018, 12, 04),
        true)
      ) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06),
        IsHalfDay = false
      };

      // Act
      eventService.UpdateEvent(eventDateDto, message, employeeId);

      // Assert
      Assert.False(eventService.GetEvent(eventId).EventDates.First().IsHalfDay);
    }

    [Fact]
    public void UpdateEvent_UpdateFromFullDayEventToHalfDay_UpdatesEventToHalfDay()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var message = "Update Message";
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> {employee};
      var eventDatesList = new List<EventDate>
      {
        Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto(
          new DateTime(2018, 12, 04),
          new DateTime(2018, 12, 04),
          false)
        )
      };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> {newEvent};

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> {eventDateWithEvent};

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06),
        IsHalfDay = true
      };

      // Act
      eventService.UpdateEvent(eventDateDto, message, employeeId);

      // Assert
      Assert.True(eventService.GetEvent(eventId).EventDates.First().IsHalfDay);
    }

    [Fact]
    public void UpdateEvent_UpdateWithNullReasonForUpdatingMessage_UpdateFailsAsExceptionIsThrown()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);
      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06),
        IsHalfDay = true
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, null, employeeId));

      // Assert
      Assert.Equal("Cannot update event, criteria not met", ex.Message);
    }

    [Fact]
    public void UpdateEvent_UpdateWithValidMessageAndIdenticalEventProperties_UpdateFailsAsExceptionIsThrown()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto(
        new DateTime(2018, 12, 04),
        new DateTime(2018, 12, 06),
        true)) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var eventDateWithEvent = Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto());
      eventDateWithEvent.Event = newEvent;
      var eventDateWithEventList = new List<EventDate> { eventDateWithEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEventDateRepository(databaseContext, eventDateWithEventList);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);
      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 04),
        EndDate = new DateTime(2018, 12, 06),
        IsHalfDay = true
      };

      // Act
      var ex = Assert.Throws<Exception>(() =>
        eventService.UpdateEvent(eventDateDto, "", employeeId));

      // Assert
      Assert.Equal("Cannot update event, criteria not met", ex.Message);
    }

    [Fact]
    public void GetHolidayStatsForUser_WithValidEmployee_ReturnsValidHolidayStatus()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      // Act
      var holidayStats = eventService.GetHolidayStatsForUser(employeeId);
      var totalHolidays = databaseContext.EmployeeRepository.GetSingle(x => x.EmployeeId == employeeId).TotalHolidays;

      // Assert
      Assert.Equal(totalHolidays, holidayStats.TotalHolidays);
    }

    [Fact]
    public void IsEventValid_WhenCalledWithoutEnoughHolidays_ReturnsNotEnoughHolidaysException()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2019, 12, 05)
      };

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.IsEventValid(eventDateDto, employeeId));

      // Assert
      Assert.Equal("Not enough holidays remaining.", ex.Message);
    }

    [Fact]
    public void IsEventValid_WhenMultipleHalfDaysConsecutivelyBooked_ReturnsCannotBookHalfDayOfMoreThanOneDayException()
    {
      // Arrange
      var eventId = 1;
      var employeeId = 1;
      var employee = TestClassBuilder.BuildGenericEmployee(null);
      var employeeList = new List<Employee> { employee };
      var eventDatesList = new List<EventDate> { Mapper.Map<EventDate>(TestClassBuilder.GenericEventDateDto()) };
      var newEvent = TestClassBuilder.BuildEvent(eventId, employeeId, TestClassBuilder.ApprovedEventStatus(),
        TestClassBuilder.AnnualLeaveEventType(), eventDatesList);
      var events = new List<Event> { newEvent };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpEventRepository(databaseContext, events);
      databaseContext = SetUpEmployeeRepository(databaseContext, employeeList);
      var eventService = GetEventService(databaseContext);

      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05),
        IsHalfDay = true
      };

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.IsEventValid(eventDateDto, employeeId));

      // Assert
      Assert.Equal("Holiday booked contains a half day whilst being more than one day.", ex.Message);
    }

    [Fact]
    public void AddMandatoryEvent_WithValidEvent_InsertsMandatoryEventIntoDb()
    {
      // Arrange
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpMandatoryEventRepository(databaseContext, new List<MandatoryEvent>());
      var eventService = GetEventService(databaseContext);
      var date = new DateTime(2020, 12, 25);
      var countryId = 1;

      // Act
      eventService.AddMandatoryEvent(date, countryId);

      // Assert
      databaseContext.Received().MandatoryEventRepository.Insert(Arg.Any<MandatoryEvent>());
    }

    [Fact]
    public void AddMandatoryEvent_WithMandatoryEventAlreadyExisting_ReturnsDateAlreadyBookedException()
    {
      // Arrange
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = 1,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.AddMandatoryEvent(date, countryId));

      // Assert
      Assert.Equal("Date is already booked as a Mandatory Event", ex.Message);
    }

    [Fact]
    public void AddMandatoryEvent_WithWeekendDate_ReturnsDateAlreadyBookedException()
    {
      // Arrange
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpMandatoryEventRepository(databaseContext, new List<MandatoryEvent>());
      var eventService = GetEventService(databaseContext);
      var date = new DateTime(2019, 03, 31);
      var countryId = 1;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.AddMandatoryEvent(date, countryId));

      // Assert
      Assert.Equal("Date is already booked as a Mandatory Event", ex.Message);
    }

    [Fact]
    public void UpdateMandatoryEvent_WithValidMandatoryEvent_UpdatesMandatoryEventInDb()
    {
      // Arrange
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = 1,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      var newEventDate = new DateTime(2019, 12, 26);

      // Act
      eventService.UpdateMandatoryEvent(mandatoryEvent.MandatoryEventId, newEventDate, countryId);

      // Assert
      databaseContext.Received().MandatoryEventRepository.Update(Arg.Any<MandatoryEvent>());
    }

    [Fact]
    public void UpdateMandatoryEvent_WithNonExistingMandatoryEventId_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      var eventId = 1;
      var invalidId = 9004;
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = eventId,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.UpdateMandatoryEvent(invalidId, date, countryId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    [Fact]
    public void UpdateMandatoryEvent_WithWeekendDate_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = 1,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      var newEventDate = new DateTime(2019, 04, 06);

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.UpdateMandatoryEvent(mandatoryEvent.MandatoryEventId, newEventDate, countryId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    [Fact]
    public void GetMandatoryEvents_WithValidCountryId_ReturnsMandatoryEvents()
    {
      // Arrange
      var eventId = 1;
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = eventId,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      var mandatoryEventsWithNorthernIrelandId = eventService.GetMandatoryEvents(countryId);
      var northernIrishMandatoryEventsCount = mandatoryEventsWithNorthernIrelandId.Count(x => x.CountryId == countryId);

      // Assert
      Assert.Equal(northernIrishMandatoryEventsCount, mandatoryEventsWithNorthernIrelandId.Count);
    }

    [Fact]
    public void GetMandatoryEvents_WithInValidCountryId_ReturnsEmptyList()
    {
      // Arrange
      var eventId = 1;
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var invalidCountryId = 9004;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = eventId,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      var mandatoryEventsWithInvalidCountryId = eventService.GetMandatoryEvents(invalidCountryId);

      // Assert
      Assert.Empty(mandatoryEventsWithInvalidCountryId);
    }

    [Fact]
    public void DeleteMandatoryEvent_WithValidMandatoryEventId_DeletesMandatoryEventFromDb()
    {
      // Arrange
      var eventId = 1;
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = eventId,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      eventService.DeleteMandatoryEvent(mandatoryEvent.MandatoryEventId);

      // Assert
      databaseContext.Received().MandatoryEventRepository.Delete(Arg.Any<MandatoryEvent>());
    }

    [Fact]
    public void DeleteMandatoryEvent_WithInValidMandatoryEventId_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      var eventId = 1;
      var invalidEventId = 9004;
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;
      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      var mandatoryEvent = new MandatoryEvent
      {
        MandatoryEventId = eventId,
        CountryId = countryId,
        MandatoryEventDate = date
      };
      var mandatoryEventList = new List<MandatoryEvent> { mandatoryEvent };
      databaseContext = SetUpMandatoryEventRepository(databaseContext, mandatoryEventList);
      var eventService = GetEventService(databaseContext);

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.DeleteMandatoryEvent(invalidEventId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    private static EventService GetEventService(IDatabaseContext databaseContext)
    {
      IDateService dateService = new DateService();
      return new EventService(databaseContext, Mapper, dateService);
    }
  }
}
