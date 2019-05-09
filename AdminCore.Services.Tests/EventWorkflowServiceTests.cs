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
using AdminCore.Common;
using AdminCore.DTOs.Employee;
using AdminCore.FsmWorkflow;
using AdminCore.Services.Tests.ClassData;
using AutoFixture;
using Xunit;
using ValidationException = AdminCore.Common.Exceptions.ValidationException;

namespace AdminCore.Services.Tests
{
  public sealed class EventWorkflowServiceTests : BaseMockedDatabaseSetUp
  {
    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EventWorkflowMapperProfile())));
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    private readonly Fixture _fixture;
    
    public EventWorkflowServiceTests()
    {
      AdminCoreContext.When(x => x.SaveChanges()).DoNotCallBase();
      
      _fixture = new Fixture();
      _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => _fixture.Behaviors.Remove(b));
      _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void CreateEvent_ValidNewEventOfOneDay_SuccessfullyInsertsNewEventIntoDb()
    {
      // Arrange
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);

      var eventTypeIdFixture = _fixture.Create<int>();
      var eventWorkflowFixture = _fixture.Create<EventWorkflow>();
      
      var fsmWorkflowHandlerMock = Substitute.For<IWorkflowFsmHandler>();
      fsmWorkflowHandlerMock.CreateEventWorkflow(Arg.Any<int>(), Arg.Any<bool>()).Returns(eventWorkflowFixture);
      
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, fsmWorkflowHandlerMock);

      // Act
      var eventWorkflowDto = eventWorkflowService.CreateEventWorkflow(eventTypeIdFixture, false);

      // Assert
      fsmWorkflowHandlerMock.Received(1).CreateEventWorkflow(Arg.Any<int>(), Arg.Any<bool>());
      Assert.NotNull(eventWorkflowDto);
    }

    #region WorkflowResponse ValidUserRole
    
    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
    public void WorkflowResponseApprove_ValidUserRoleForEventType_CallMadeToFsmHandler(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      eventDto.EmployeeId = employeeDto.EmployeeId;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());

      var workflowFsmStateInfoMock = _fixture.Create<WorkflowFsmStateInfo>();
      
      var fsmWorkflowHandlerMock = Substitute.For<IWorkflowFsmHandler>();
      fsmWorkflowHandlerMock.FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>()).Returns(workflowFsmStateInfoMock);
      
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, fsmWorkflowHandlerMock);

      // Act
      var workflowFsmStateInfo = eventWorkflowService.WorkflowResponseApprove(eventDto, employeeDto);
      
      // Assert
      Assert.NotNull(workflowFsmStateInfo);
      fsmWorkflowHandlerMock.Received(1).FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>());
    }
    
    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
    public void WorkflowResponseReject_ValidUserRoleForEventType_CallMadeToFsmHandler(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      eventDto.EmployeeId = employeeDto.EmployeeId;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());

      var workflowFsmStateInfoMock = _fixture.Create<WorkflowFsmStateInfo>();
      
      var fsmWorkflowHandlerMock = Substitute.For<IWorkflowFsmHandler>();
      fsmWorkflowHandlerMock.FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>()).Returns(workflowFsmStateInfoMock);
      
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, fsmWorkflowHandlerMock);

      // Act
      var workflowFsmStateInfo = eventWorkflowService.WorkflowResponseReject(eventDto, employeeDto);
      
      // Assert
      Assert.NotNull(workflowFsmStateInfo);
      fsmWorkflowHandlerMock.Received(1).FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>());
    }

        [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler))]
    public void WorkflowResponseCancel_ValidUserRoleForEventType_CallMadeToFsmHandler(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      eventDto.EmployeeId = employeeDto.EmployeeId;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());

      var workflowFsmStateInfoMock = _fixture.Create<WorkflowFsmStateInfo>();
      
      var fsmWorkflowHandlerMock = Substitute.For<IWorkflowFsmHandler>();
      fsmWorkflowHandlerMock.FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>()).Returns(workflowFsmStateInfoMock);
      
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, fsmWorkflowHandlerMock);

      // Act
      var workflowFsmStateInfo = eventWorkflowService.WorkflowResponseCancel(eventDto, employeeDto);
      
      // Assert
      Assert.NotNull(workflowFsmStateInfo);
      fsmWorkflowHandlerMock.Received(1).FireLeaveResponse(Arg.Any<EventDto>(), Arg.Any<EmployeeDto>(), Arg.Any<EventStatuses>(), Arg.Any<EventWorkflow>());
    }
    
    #endregion
    
    #region WorkflowResponse InvalidUserRole
    
    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException))]
    public void WorkflowResponseApprove_InvalidUserRoleForEventType_ThrowsValidationException(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      eventDto.EmployeeId = employeeDto.EmployeeId;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());
           
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, null);

      // Act
      // Assert
      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseApprove(eventDto, employeeDto));
    }

    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException))]
    public void WorkflowResponseReject_InvalidUserRoleForEventType_ThrowsValidationException(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      eventDto.EmployeeId = employeeDto.EmployeeId;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());
           
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, null);

      // Act
      // Assert
      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseReject(eventDto, employeeDto));
    }
    
    [Theory]
    [ClassData(typeof(EventWorkflowServiceClassData.WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException))]
    public void WorkflowResponseCancel_InvalidUserRoleForEventType_ThrowsValidationException(
      int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList)
    {
      // Arrange
      var eventDto = _fixture.Create<EventDto>();
      // Event must be created by same same user as sending the cancel response. Set as + 1 to ensure a failure.
      eventDto.EmployeeId = employeeDto.EmployeeId + 1;
      eventDto.EventTypeId = eventTypeId;
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventTypeRequiredRespondersRepository(databaseContextMock, eventTypeRequiredRespondersList);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
      databaseContextMock = SetUpEmployeeApprovalResponseRepository(databaseContextMock, new List<EmployeeApprovalResponse>());
           
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, null);

      // Act
      // Assert
      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseCancel(eventDto, employeeDto));
    }
    
    #endregion
    
    [Fact]
    public void WorkflowResponseApprove_EventWorkflowWithIdDoesNotExistInDb_ThrowsValidationException()
    {
      // Arrange      
      var employeeDto = _fixture.Create<EmployeeDto>();
      var eventDto = _fixture.Create<EventDto>();
      var eventWorkflowDto = _fixture.Create<EventWorkflow>();
      // EventWorkflow id does not match foreign key in Event.
      eventWorkflowDto.EventWorkflowId = eventDto.EventWorkflowId + 1;
      
      var databaseContextMock = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContextMock = SetUpEventWorkflowRepository(databaseContextMock, new List<EventWorkflow> {eventWorkflowDto});
           
      var eventWorkflowService = new EventWorkflowService(databaseContextMock, Mapper, null);

      // Act
      // Assert
      Assert.Throws<ValidationException>(() => eventWorkflowService.WorkflowResponseApprove(eventDto, employeeDto));
    }
  }
}