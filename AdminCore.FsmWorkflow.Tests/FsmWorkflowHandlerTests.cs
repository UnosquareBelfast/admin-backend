//
//
//using System.Collections.Generic;
//using System.Linq;
//using AdminCore.Common;
//using AdminCore.Common.Exceptions;
//using AdminCore.Common.Interfaces;
//using AdminCore.Constants.Enums;
//using AdminCore.DAL.Database;
//using AdminCore.DAL.Entity_Framework;
//using AdminCore.DAL.Models;
//using AdminCore.DTOs.Employee;
//using AdminCore.DTOs.Event;
//using AdminCore.FsmWorkflow;
//using AdminCore.FsmWorkflow.Factory;
//using AdminCore.FsmWorkflow.FsmMachines;
//using AdminCore.FsmWorkflow.FsmMachines.FsmWorkflowState;
//using AdminCore.Services.Tests.ClassData;
//using AutoFixture;
//using NSubstitute;
//using Xunit;
//
//namespace AdminCore.Services.Tests
//{
//  public sealed class FsmWorkflowHandlerTests : BaseMockedDatabaseSetUp
//  {
//    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Substitute.For<IConfiguration>());
//
//    private readonly Fixture _fixture;
//
//    public FsmWorkflowHandlerTests()
//    {
//      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
//
//      _fixture = new Fixture();
//      _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//        .ForEach(b => _fixture.Behaviors.Remove(b));
//      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//    }
//
//    [Theory]
//    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb))]
//    public void CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb(int eventTypeId)
//    {
//      // Arrange
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
//
//      var fsmWorkflowHandler = new WorkflowFsmHandler(databaseContextMock, null);
//
//      // Act
//      fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, true);
//
//      // Assert
//      databaseContextMock.Received(1).EventWorkflowRepository.Insert(Arg.Any<EventWorkflow>());
//      databaseContextMock.Received(1).SaveChanges();
//    }
//
//    [Theory]
//    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_SaveChangesToDbContextIsTrue_NewEventWorkflowIsInsertedIntoDb))]
//    public void CreateEventWorkflow_SaveChangesToDbContextIsFalse_NewEventWorkflowNotInsertedIntoDb(int eventTypeId)
//    {
//      // Arrange
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
//
//      var fsmWorkflowHandler = new WorkflowFsmHandler(databaseContextMock, null);
//
//      // Act
//      fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, false);
//
//      // Assert
//      databaseContextMock.Received(1).EventWorkflowRepository.Insert(Arg.Any<EventWorkflow>());
//      databaseContextMock.DidNotReceive().SaveChanges();
//    }
//
//    [Theory]
//    [ClassData(typeof(FsmWorkflowHandlerData.CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflow_ThrowsWorkflowException))]
//    public void CreateEventWorkflow_EventTypesProvidedDoNotHaveAssociatedWorkflow_ThrowsWorkflowException(int eventTypeId)
//    {
//      // Arrange
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
//
//      var fsmWorkflowHandler = new WorkflowFsmHandler(databaseContextMock, null);
//
//      // Act
//      // Assert
//      Assert.Throws<WorkflowException>(() => fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, false));
//    }
//
//    [Theory]
//    [InlineData((int)EventTypes.AnnualLeave)]
//    [InlineData((int)EventTypes.WorkingFromHome)]
//    public void FireLeaveResponse_ValidEventIdApprove_EventApproved(int eventTypeId)
//    {
//      // Arrange
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
//
//      var leaveWorkflow = Substitute.For<ILeaveWorkflow>();
//      leaveWorkflow.FireLeaveResponded(Arg.Any<EventStatuses>(), Arg.Any<string>()).Returns(_fixture.Create<WorkflowFsmStateInfo>());
//
//      var workflowFsmFactoryMock = Substitute.For<IWorkflowFsmFactory<ILeaveWorkflow>>();
//      workflowFsmFactoryMock.GetWorkflowPto(Arg.Any<WorkflowStateData>()).Returns(leaveWorkflow);
//      workflowFsmFactoryMock.GetWorkflowWfh(Arg.Any<WorkflowStateData>()).Returns(leaveWorkflow);
//
//      var employeeEvent = _fixture.Create<EventDto>();
//      employeeEvent.EventTypeId = eventTypeId;
//      var employeeDto = _fixture.Create<EmployeeDto>();
//      var eventStatus = _fixture.Create<EventStatuses>();
//      var eventWorkflow = _fixture.Create<EventWorkflow>();
//      eventWorkflow.EventWorkflowApprovalResponses = new List<SystemUserApprovalResponse>();
//
//      var fsmWorkflowHandler = new WorkflowFsmHandler(databaseContextMock, workflowFsmFactoryMock);
//
//      // Act
//      var workflowFsmStateinfo = fsmWorkflowHandler.FireLeaveResponse(employeeEvent, employeeDto, eventStatus, eventWorkflow);
//
//      // Assert
//      databaseContextMock.Received(1).EventWorkflowRepository.Update(Arg.Any<EventWorkflow>());
//      databaseContextMock.Received(1).SaveChanges();
//      Assert.Equal(1, eventWorkflow.EventWorkflowApprovalResponses.Count);
//    }
//
//    [Theory]
//    [InlineData(420)]
//    [InlineData(69)]
//    public void FireLeaveResponse_EventTypesProvidedDoNotHaveAssociatedWorkflows_ThrowsWorkflowException(
//      int eventTypeId)
//    {
//      // Arrange
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow>());
//
//      var fsmWorkflowHandler = new WorkflowFsmHandler(databaseContextMock, null);
//
//      var employeeEvent = _fixture.Create<EventDto>();
//      employeeEvent.EventTypeId = eventTypeId;
//      var employeeDto = _fixture.Create<EmployeeDto>();
//      var eventStatus = _fixture.Create<EventStatuses>();
//      var eventWorkflow = _fixture.Create<EventWorkflow>();
//
//      // Act
//      // Assert
//      Assert.Throws<WorkflowException>(() => fsmWorkflowHandler.FireLeaveResponse(employeeEvent, employeeDto, eventStatus, eventWorkflow));
//    }
//  }
//}
