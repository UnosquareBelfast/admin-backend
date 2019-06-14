using System;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Event;
using AdminCore.Services.Mappings;
using AutoMapper;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.Common;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.SystemUser;
using AdminCore.FsmWorkflow;
using AdminCore.Services.Tests.ClassData;
using AdminCore.WebApi.Mappings;
using AutoFixture;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using Xunit;
using ValidationException = AdminCore.Common.Exceptions.ValidationException;

namespace AdminCore.Services.Tests
{
  public sealed class EventWorkflowServiceTests : BaseMockedDatabaseSetUp
  {
    private readonly Fixture _fixture;

    public EventWorkflowServiceTests()
    {
      _fixture = new Fixture();
      _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => _fixture.Behaviors.Remove(b));
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      var eventTypeIdFixture = _fixture.Create<int>();
      var eventWorkflowFixture = _fixture.Create<EventWorkflow>();

      var eventWorkflowService = GetMockedResourcesCreateWorkflow(eventWorkflowFixture, out var ormContext, out var mapper, out var workflowFsmHandler);

      // Act
      var eventWorkflowActual = eventWorkflowService.CreateEventWorkflow(eventTypeIdFixture, false);

      // Assert
      workflowFsmHandler.Received(1).CreateEventWorkflow(Arg.Any<int>(), Arg.Any<bool>());

      eventWorkflowActual.Should().NotBeNull();
    }

    #region WorkflowResponse ValidUserRole

    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
    public void WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler(int systemUserId, EventDto eventDto, EventStatuses eventStatuses,
      IList<SystemUser> systemUsersDbReturns, IList<Employee> employeeDbReturns, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersDbReturns, IList<EventWorkflow> eventWorkflowDbReturns,
      IList<SystemUserApprovalResponse> systemUserApprovalResponseDbReturns)
    {
      // Arrange
      var eventWorkflowService = GetMockedResourcesWorkflowResponse(systemUsersDbReturns, employeeDbReturns, eventTypeRequiredRespondersDbReturns,
        eventWorkflowDbReturns, systemUserApprovalResponseDbReturns, out var ormContext, out var mapper, out var workflowFsmHandler);

      // Act
      var workflowFsmStateInfoActual = eventWorkflowService.WorkflowResponse(eventDto, systemUserId, eventStatuses);

      // Assert
      workflowFsmHandler.Received(1).FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<SystemUser>(), Arg.Any<SystemUserRoles>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>());
      ormContext.ReceivedWithAnyArgs(1).SystemUserRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).EmployeeRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).EventWorkflowRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).SystemUserApprovalResponsesRepository.Get();

      Assert.NotNull(workflowFsmStateInfoActual);
    }

    #endregion

    #region WorkflowResponse InvalidUserRole

    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException))]
    public void WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException(int systemUserId, EventDto eventDto, EventStatuses eventStatuses,
      IList<SystemUser> systemUsersDbReturns, IList<Employee> employeeDbReturns, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersDbReturns, IList<EventWorkflow> eventWorkflowDbReturns,
      IList<SystemUserApprovalResponse> systemUserApprovalResponseDbReturns)
    {
      // Arrange
      var eventWorkflowService = GetMockedResourcesWorkflowResponse(systemUsersDbReturns, employeeDbReturns, eventTypeRequiredRespondersDbReturns,
        eventWorkflowDbReturns, systemUserApprovalResponseDbReturns, out var ormContext, out var mapper, out var workflowFsmHandler);

      // Act
      Action workflowResponseFunc = () => eventWorkflowService.WorkflowResponse(eventDto, systemUserId, eventStatuses);

      // Assert
      workflowResponseFunc.Should().Throw<ValidationException>();

      ormContext.ReceivedWithAnyArgs(1).SystemUserRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).EmployeeRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).EventWorkflowRepository.GetSingle();
      ormContext.ReceivedWithAnyArgs(1).SystemUserApprovalResponsesRepository.Get();
    }

    #endregion

    #region WorkflowResponseCancel EmployeeIdCorrect

//    [Theory]
//    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
//    public void WorkflowResponseCancel_EmployeeCancelsEventBySameEmployee_CallMadeToFsmHandler(
//      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
//    {
//      // Arrange
//      var eventDto = _fixture.Create<EventDto>();
//      var workflowFsmHandlerMock = Substitute.For<IWorkflowFsmHandler>();
//      var eventWorkflowService = ConstructEventWorkflowService_ForWorkflowResponse(eventTypeId, employeeDto, eventTypeRequiredRespondersList,
//        eventDto, workflowFsmHandlerMock, employeeDto.EmployeeId);
//
//      // Act
//      var workflowFsmStateInfo = eventWorkflowService.WorkflowResponseCancel(eventDto, employeeDto);
//
//      // Assert
//      Assert.NotNull(workflowFsmStateInfo);
//      workflowFsmHandlerMock.Received(1).FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>());
//    }

    #endregion

//    #region WorkflowResponseApprove/Reject/Cancel EmployeeIdIncorrect
//
//        [Theory]
//    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
//    public void WorkflowResponseApprove_EmployeeCancelsOtherEmployeeEvent_CallMadeToFsmHandler(
//      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
//    {
//      // Arrange
//      var eventDto = _fixture.Create<EventDto>();
//      var workflowFsmHandlerMock = Substitute.For<IWorkflowFsmHandler>();
//      var eventWorkflowService = ConstructEventWorkflowService_ForWorkflowResponse(eventTypeId, employeeDto, eventTypeRequiredRespondersList,
//        eventDto, workflowFsmHandlerMock, employeeDto.EmployeeId);
//
//      // Act
//      // Assert
//      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponse(eventDto, employeeDto));
//    }
//
//    [Theory]
//    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
//    public void WorkflowResponseReject_EmployeeCancelsOtherEmployeeEvent_CallMadeToFsmHandler(
//      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
//    {
//      // Arrange
//      var eventDto = _fixture.Create<EventDto>();
//      var workflowFsmHandlerMock = Substitute.For<IWorkflowFsmHandler>();
//      var eventWorkflowService = ConstructEventWorkflowService_ForWorkflowResponse(eventTypeId, employeeDto, eventTypeRequiredRespondersList,
//        eventDto, workflowFsmHandlerMock, employeeDto.EmployeeId);
//
//      // Act
//      // Assert
//      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseReject(eventDto, employeeDto));
//    }
//
//    [Theory]
//    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
//    public void WorkflowResponseCancel_EmployeeCancelsOtherEmployeeEvent_ThrowsValidationException(
//      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
//    {
//      // Arrange
//      var eventDto = _fixture.Create<EventDto>();
//      var workflowFsmHandlerMock = Substitute.For<IWorkflowFsmHandler>();
//      var eventWorkflowService = ConstructEventWorkflowService_ForWorkflowResponse(eventTypeId, employeeDto, eventTypeRequiredRespondersList,
//        eventDto, workflowFsmHandlerMock, employeeDto.EmployeeId + 1);
//
//      // Act
//      // Assert
//      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseCancel(eventDto, employeeDto));
//    }
//
//    #endregion
//
//    [Fact]
//    public void WorkflowResponseApprove_EventWorkflowWithIdDoesNotExistInDb_ThrowsValidationException()
//    {
//      // Arrange
//      var employeeDto = _fixture.Create<EmployeeDto>();
//      var eventDto = _fixture.Create<EventDto>();
//      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
//      // EventWorkflow id does not match foreign key in Event.
//      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId + 1;
//
//      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
//      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
//
//      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, null);
//
//      // Act
//      // Assert
//      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponse(eventDto, employeeDto));
//    }

    #region MockCreation

        private EventWorkflowService GetMockedResourcesCreateWorkflow(EventWorkflow eventWorkflow, out EntityFrameworkContext ormContext, out IMapper mapper,
      out IWorkflowFsmHandler workflowFsmHandler)
    {
      var efContext = SetupMockedOrmContext(out var dbContext);
      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new SystemUserMappingProfile());
        cfg.AddProfile(new EventWorkflowMapperProfile());
      })).Configure();

      workflowFsmHandler = Substitute.For<IWorkflowFsmHandler>();
      workflowFsmHandler.CreateEventWorkflow(Arg.Any<int>(), Arg.Any<bool>()).Returns(eventWorkflow);

      ormContext = efContext;
      return new EventWorkflowService(ormContext, mapper, workflowFsmHandler);
    }

    private EventWorkflowService GetMockedResourcesWorkflowResponse(IList<SystemUser> systemUserReturns, IList<Employee> employeeReturns,
      IList<EventTypeRequiredResponders> eventTypeRequiredRespondersReturns, IList<EventWorkflow> eventWorkflowReturns,
      IList<SystemUserApprovalResponse> systemUserApprovalResponseReturns, out EntityFrameworkContext ormContext, out IMapper mapper,
      out IWorkflowFsmHandler workflowFsmHandler)
    {
      var efContext = SetupMockedOrmContext(out var dbContext);
      efContext = SetUpGenericRepository(efContext, systemUserReturns,
        repository => { efContext.Configure().SystemUserRepository.Returns(repository); }, dbContext);
      efContext = SetUpGenericRepository(efContext, employeeReturns,
        repository => { efContext.Configure().EmployeeRepository.Returns(repository); }, dbContext);
      efContext = SetUpGenericRepository(efContext, eventTypeRequiredRespondersReturns,
        repository => { efContext.Configure().EventTypeRequiredRespondersRepository.Returns(repository); }, dbContext);
      efContext = SetUpGenericRepository(efContext, eventWorkflowReturns,
        repository => { efContext.Configure().EventWorkflowRepository.Returns(repository); }, dbContext);
      efContext = SetUpGenericRepository(efContext, systemUserApprovalResponseReturns,
        repository => { efContext.Configure().SystemUserApprovalResponsesRepository.Returns(repository); }, dbContext);

      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new SystemUserMappingProfile());
        cfg.AddProfile(new EventWorkflowMapperProfile());
      })).Configure();

      workflowFsmHandler = Substitute.For<IWorkflowFsmHandler>();
      workflowFsmHandler.FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<SystemUser>(), Arg.Any<SystemUserRoles>(),
          Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>())
        .Returns(new WorkflowFsmStateInfo(false, EventStatuses.AwaitingApproval, ""));

      ormContext = efContext;
      return new EventWorkflowService(ormContext, mapper, workflowFsmHandler);
    }

    #endregion
  }
}
