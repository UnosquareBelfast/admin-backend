using AdminCore.Common.Interfaces;
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
using AdminCore.FsmWorkflow;
using AdminCore.Services.Tests.ClassData;
using Microsoft.AspNetCore.Http.Features;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class EventWorkflowServiceTests : BaseMockedDatabaseSetUp
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    public EventWorkflowServiceTests()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
    }

    [Theory]
    [ClassData(typeof(EventServiceClassData.CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb_ClassData))]
    public void CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb(
      int employeeId, int eventId, DateTime startDate, DateTime endDate, EventType eventType, IList<EventTypeDaysNotice> eventTypeDaysNoticeList, DateTime dateServiceNow)
    {
      // Arrange
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
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);

      var fsmWorkflowHandlerMock = Substitute.For<IFsmWorkflowHandler>();
      fsmWorkflowHandlerMock.CreateEventWorkflow(eventId, Arg.Any<bool>()).Returns(
        new EventWorkflow
        {
          EventWorkflowId = 0,
          EventWorkflowApprovalResponses = null,
          WorkflowState = 0
        });
      
      var eventService = new EventWorkflowService(databaseContextMock, Mapper, fsmWorkflowHandlerMock);

      // Act
      eventService.CreateEventWorkflow(eventType.EventTypeId, false);

      // Assert
      databaseContextMock.Received().EventRepository.Insert(Arg.Any<Event>());
    }
  }
}