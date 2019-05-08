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
using AdminCore.Common.Exceptions;
using AdminCore.DTOs.Employee;
using AdminCore.FsmWorkflow;
using AdminCore.Services.Tests.ClassData;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http.Features;
using Xunit;

namespace AdminCore.Services.Tests
{
  public sealed class FsmWorkflowHandlerTests : BaseMockedDatabaseSetUp
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventMapperProfile())));
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    public FsmWorkflowHandlerTests()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
    }

    [Theory]
    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb))]
    public void CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb(int eventTypeId)
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
      
      var fsmWorkflowHandler = new FsmWorkflowHandler(databaseContextMock);

      // Act
      fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, true);

      // Assert
      databaseContextMock.Received(1).EventWorkflowRepository.Insert(Arg.Any<EventWorkflow>());
      databaseContextMock.Received(1).SaveChanges();
    }
    
    [Theory]
    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb))]
    public void CreateEventWorkflow_SaveChangesToDbContextIsFalse_NewEventWorkflowNotInsertedIntoDb(int eventTypeId)
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
      
      var fsmWorkflowHandler = new FsmWorkflowHandler(databaseContextMock);
      
      // Act
      fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, false);

      // Assert
      databaseContextMock.Received(1).EventWorkflowRepository.Insert(Arg.Any<EventWorkflow>());
      databaseContextMock.DidNotReceive().SaveChanges();
    }
    
    [Theory]
    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflows_ThrowsWorkflowException))]
    public void CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflows_ThrowsWorkflowException(int eventTypeId)
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
      
      var fsmWorkflowHandler = new FsmWorkflowHandler(databaseContextMock);
      
      // Act
      // Assert
      Assert.Throws<WorkflowException>(() => fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, false));
    }
    
//    [Theory]
//    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflows_ThrowsWorkflowException))]
    [Theory]
    [InlineData((int)EventTypes.AnnualLeave)]
    [InlineData((int)EventTypes.WorkingFromHome)]
    public void FireLeaveResponse_ValidEventIdApprove_EventApproved(int eventTypeId)
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
           
      var fixture = new Fixture();
      fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));
      fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
      var employeeEvent = fixture.Create<EventDto>();
      employeeEvent.EventTypeId = eventTypeId;
      var employeeDto = fixture.Create<EmployeeDto>();
      var eventStatus = fixture.Create<EventStatuses>();
      var eventWorkflow = fixture.Create<EventWorkflow>();
      
      var fsmWorkflowHandler = new FsmWorkflowHandler(databaseContextMock);
      
      // Act
      var workflowFsmStateinfo = fsmWorkflowHandler.FireLeaveResponse(employeeEvent, employeeDto, eventStatus, eventWorkflow);

      // Assert
      databaseContextMock.Received(1).EventWorkflowRepository.Update(Arg.Any<EventWorkflow>());
      databaseContextMock.Received(1).SaveChanges();
    }
    
    [Theory]
    [InlineData((int)EventTypes.Wedding)]
    [InlineData((int)EventTypes.Sickness)]
    public void FireLeaveResponse_EventTypesProvidedDoNotHaveAssociatedWorkflows_ThrowsWorkflowException(
      int eventTypeId)
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
      
      var fsmWorkflowHandler = new FsmWorkflowHandler(databaseContextMock);
      
      var fixture = new Fixture();
      fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));
      fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
      var employeeEvent = fixture.Create<EventDto>();
      employeeEvent.EventTypeId = eventTypeId;
      var employeeDto = fixture.Create<EmployeeDto>();
      var eventStatus = fixture.Create<EventStatuses>();
      var eventWorkflow = fixture.Create<EventWorkflow>();
      
      // Act
      // Assert
      Assert.Throws<WorkflowException>(() => fsmWorkflowHandler.FireLeaveResponse(employeeEvent, employeeDto, eventStatus, eventWorkflow));
    }
  }
}