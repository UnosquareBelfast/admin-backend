using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Services.Mappings;
using AutoMapper;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class EventServiceTests : BaseMockedDatabaseSetUp
  {
    [Fact]
    public void CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 03)
      };

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneHalfDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 03),
        IsHalfDay = true
      };

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_WhenEventDatesAlreadyBooked_ThrowsAlreadyBookedException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 2;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 18),
        EndDate = new DateTime(2018, 12, 21)
      };

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId));

      // Assert
      Assert.Equal("Holiday dates already booked.", ex.Message);
    }

    [Fact]
    public void CreateEvent_ValidNewEvent_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_WithSickLeaveEventType_SuccessfullyInsertsNewEventIntoDbAndIsAutoApproved()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.Sickness, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void CreateEvent_WithoutEnoughAvailableHolidays_ThrowsNotEnoughHolidaysException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2019, 12, 05)
      };

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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      const int employeeId = 1;
      var eventType = EventTypes.PaternityLeave;
      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.CreateEvent(eventDateDto, eventType, employeeId));

      // Assert
      Assert.Equal("User does not have the correct privileges to book this type of event.", ex.Message);
    }

    [Fact]
    public void CreateEvent_WithValidEmployee_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employee = new Employee
      {
        EmployeeId = 6,
        CountryId = 1,
        EmployeeRoleId = 1,
        EmployeeStatusId = 1,
        Forename = "FirstName",
        StartDate = DateTime.Today,
        Surname = "Surname",
        TotalHolidays = 50
      };

      var eventDateDto = new EventDateDto
      {
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      eventService.CreateEvent(eventDateDto, EventTypes.AnnualLeave, employee);

      // Assert
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void GetEmployeeEvents_WithAnnualLeaveEventType_ReturnsListOfEmployeeAnnualLeaveEvents()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventType = EventTypes.AnnualLeave;

      // Act
      var annualLeaveEventsList = eventService.GetEmployeeEvents(eventType);

      // Assert
      Assert.Equal(14, annualLeaveEventsList.Count);
    }

    [Fact]
    public void GetEmployeeEvents_WithSickLeaveEventType_ReturnsListOfEmployeeSickLeaveEvents()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventType = EventTypes.Sickness;

      // Act
      var sickLeaveEventsList = eventService.GetEmployeeEvents(eventType);

      // Assert
      Assert.Equal(2, sickLeaveEventsList.Count);
    }

    [Fact]
    public void GetByDateBetween_WithValidDateAndAnnualLeaveEventType_ReturnsListOfAnnualLeaveEventsBetweenDatesGiven()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      DateTime startDate = new DateTime(2017, 12, 03);
      DateTime endDate = new DateTime(2019, 12, 01);
      var eventType = EventTypes.AnnualLeave;

      // Act
      var annualLeaveEventsByDateList = eventService.GetByDateBetween(startDate, endDate, eventType);

      // Assert
      Assert.Equal(32, annualLeaveEventsByDateList.Count);
    }

    [Fact]
    public void GetEmployeeEventsById_WithValidEmployeeAndAnnualLeaveEventType_ReturnsListOfAnnualLeaveEventsForThatEmployee()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employeeId = 1;
      var eventType = EventTypes.AnnualLeave;

      // Act
      var eventsByEmployeeId = eventService.GetEventsByEmployeeId(employeeId, eventType);

      // Assert
      Assert.Equal(3, eventsByEmployeeId.Count);
    }

    [Fact]
    public void GetEmployeeEventsById_WithValidEmployeeAndSickLeaveEventType_ReturnsListOfEventsForThatEmployee()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employeeId = 1;
      var eventType = EventTypes.Sickness;

      // Act
      var eventsByEmployeeId = eventService.GetEventsByEmployeeId(employeeId, eventType);

      // Assert
      Assert.Equal(1, eventsByEmployeeId.Count);
    }

    [Fact]
    public void
      GetApprovedEventDatesByEmployeeIdAndStartAndEndDate_WithValidDates_ReturnsListOfEventDatesByEmployeeIdAndStartAndEndDate()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      DateTime startDate = new DateTime(2017, 12, 03);
      DateTime endDate = new DateTime(2019, 12, 01);
      var employeeId = 1;

      // Act
      var eventDateList =
        eventService.GetApprovedEventDatesByEmployeeAndStartAndEndDates(startDate, endDate, employeeId);

      // Assert
      Assert.Equal(4, eventDateList.Count);
    }

    [Fact]
    public void GetEventById_WithExistingEventId_ReturnsEvent()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;

      // Act
      var returnedEvent = eventService.GetEvent(eventId);

      // Assert
      Assert.Equal(eventId, returnedEvent.EventId);
    }

    [Fact]
    public void GetEventById_WithNonExistingEventId_ReturnsNull()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 9004;

      // Act
      var returnedEvent = eventService.GetEvent(eventId);

      // Assert
      Assert.Null(returnedEvent);
    }

    [Fact]
    public void GetEventByStatus_WithApprovedEventStatus_ReturnsAllEventsOfApprovedStatusType()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventStatus = EventStatuses.Approved;
      var eventType = EventTypes.AnnualLeave;

      // Act
      var returnedEvents = eventService.GetEventByStatus(eventStatus, eventType);

      // Assert
      Assert.Equal(11, returnedEvents.Count);
    }

    [Fact]
    public void GetEventByType_WithAnnualLeaveEventType_ReturnsListOfAllAnnualLeaveEvents()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var annualLeaveId = EventTypes.AnnualLeave;

      // Act
      var returnedEvents = eventService.GetEventByType(annualLeaveId);

      // Assert
      Assert.Equal(13, returnedEvents.Count);
    }

    [Fact]
    public void RejectEvent_WithValidEvent_RejectsTheEventOfEventIdProvided()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
      var message = "Rejected";
      var employeeIdRejecting = 2;

      // Act
      eventService.RejectEvent(eventId, message, employeeIdRejecting);
      var ex = Assert.Throws<Exception>(() => eventService.RejectEvent(eventId, message, employeeIdRejecting));

      // Assert
      databaseContext.Received().SaveChanges();
      Assert.Equal($"Event {eventId} doesn't exist or is already rejected", ex.Message);
    }

    [Fact]
    public void RejectEvent_WhenCalledWithEventIdNotExisting_ThrowsException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 9308315;
      var message = "Rejected";
      var employeeIdRejecting = 2;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.RejectEvent(eventId, message, employeeIdRejecting));

      // Assert
      Assert.Equal($"Event {eventId} doesn't exist or is already rejected", ex.Message);
    }

    [Fact]
    public void UpdateEventStatus_WithUpdatedStatusBeingCancelled_UpdatesEventStatusToCancelled()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
      var eventStatus = EventStatuses.Cancelled;

      // Act
      eventService.UpdateEventStatus(eventId, eventStatus);

      // Assert
      Assert.Equal((int)eventStatus, eventService.GetEvent(eventId).EventStatusId);
    }

    [Fact]
    public void UpdateEventStatus_WhenCalledOnEventThatCannotHaveEventStatusUpdated_DoesNotUpdatesEventStatusForMandatoryEvent()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 16;
      var eventStatus = EventStatuses.Cancelled;

      // Act
      eventService.UpdateEventStatus(eventId, eventStatus);

      // Assert
      Assert.NotEqual((int)eventStatus, eventService.GetEvent(eventId).EventStatusId);
    }

    [Fact]
    public void UpdateEvent_WhenCalled_UpdatesTheEventWithNewDates()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
      var employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      eventService.UpdateEvent(eventDateDto, null, employeeId);

      // Assert
      databaseContext.Received().EventRepository.Delete(Arg.Any<Event>());
      databaseContext.Received().EventRepository.Insert(Arg.Any<Event>());
    }

    [Fact]
    public void UpdateEvent_WhenCalledOnEventThatCannotBeUpdated_DoesNotTheMandatoryEventWithNewDates()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 16;
      var oldMessageCount = eventService.GetEvent(eventId).EventMessages.Count;
      var employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2018, 12, 05)
      };

      // Act
      eventService.UpdateEvent(eventDateDto, null, employeeId);

      // Assert
      Assert.Equal(oldMessageCount, eventService.GetEvent(eventId).EventMessages.Count);
    }

    [Fact]
    public void UpdateEvent_WhenNotEnoughHolidays_DoesNotUpdateEvent()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var eventId = 1;
      var message = "Updated";
      var employeeId = 1;
      var eventDateDto = new EventDateDto
      {
        EventId = eventId,
        StartDate = new DateTime(2018, 12, 03),
        EndDate = new DateTime(2019, 12, 05)
      };

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.UpdateEvent(eventDateDto, message, employeeId));

      // Assert
      Assert.Equal("Not enough holidays to book", ex.Message);
    }

    [Fact]
    public void GetHolidayStatsForUser_WithValidEmployee_ReturnsValidHolidayStatus()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employeeId = 1;

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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employeeId = 1;
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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var employeeId = 1;
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
      IDatabaseContext databaseContext = SetUpDatabase();
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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var date = new DateTime(2019, 12, 25);
      var countryId = 1;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.AddMandatoryEvent(date, countryId));

      // Assert
      Assert.Equal("Date is already booked as a Mandatory Event", ex.Message);
    }

    [Fact]
    public void AddMandatoryEvent_WithWeekendDate_ReturnsDateAlreadyBookedException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var date = new DateTime(2019, 03, 24);
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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var mandatoryEvent = 1;
      var date = new DateTime(2019, 03, 27);
      var countryId = 1;

      // Act
      eventService.UpdateMandatoryEvent(mandatoryEvent, date, countryId);

      // Assert
      databaseContext.Received().MandatoryEventRepository.Update(Arg.Any<MandatoryEvent>());
    }

    [Fact]
    public void UpdateMandatoryEvent_WithNonExistingMandatoryEventId_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var mandatoryEvent = 9004;
      var date = new DateTime(2019, 03, 27);
      var countryId = 1;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.UpdateMandatoryEvent(mandatoryEvent, date, countryId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    [Fact]
    public void UpdateMandatoryEvent_WithWeekendDate_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var mandatoryEvent = 1;
      var date = new DateTime(2019, 03, 24);
      var countryId = 1;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.UpdateMandatoryEvent(mandatoryEvent, date, countryId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    [Fact]
    public void GetMandatoryEvents_WithValidCountryId_ReturnsMandatoryEvents()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var countryId = 1;

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
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var countryId = 9004;

      // Act
      var mandatoryEventsWithInvalidCountryId = eventService.GetMandatoryEvents(countryId);

      // Assert
      Assert.Empty(mandatoryEventsWithInvalidCountryId);
    }

    [Fact]
    public void DeleteMandatoryEvent_WithValidMandatoryEventId_DeletesMandatoryEventFromDb()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var mandatoryEventId = 1;

      // Act
      eventService.DeleteMandatoryEvent(mandatoryEventId);

      // Assert
      databaseContext.Received().MandatoryEventRepository.Delete(Arg.Any<MandatoryEvent>());
    }

    [Fact]
    public void DeleteMandatoryEvent_WithInValidMandatoryEventId_ReturnsMandatoryEventDoesNotExistException()
    {
      // Arrange
      IDatabaseContext databaseContext = SetUpDatabase();
      var eventService = GetEventService(databaseContext);
      var mandatoryEventId = 9004;

      // Act
      var ex = Assert.Throws<Exception>(() => eventService.DeleteMandatoryEvent(mandatoryEventId));

      // Assert
      Assert.Equal("Mandatory Event does not exist", ex.Message);
    }

    private static EventService GetEventService(IDatabaseContext databaseContext)
    {
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));
      IDateService dateService = new DateService();
      return new EventService(databaseContext, mapper, dateService);
    }
  }
}