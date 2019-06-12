using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AdminCore.Common;
using AdminCore.Common.Exceptions;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.EventWorkflow;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.FsmWorkflow;
using AdminCore.Services.Base;
using AutoMapper;

namespace AdminCore.Services
{
    public class EventWorkflowService : BaseService, IEventWorkflowService
    {
        private readonly IMapper _mapper;
        private readonly IWorkflowFsmHandler _workflowFsmHandler;

        public EventWorkflowService(IDatabaseContext databaseContext, IMapper mapper, IWorkflowFsmHandler workflowFsmHandler) : base(databaseContext)
        {
            _mapper = mapper;
            _workflowFsmHandler = workflowFsmHandler;
        }

        public EventWorkflowDto CreateEventWorkflow(int eventTypeId, bool saveChangesToDbContext = true)
        {
            var newEventWorkflow = _workflowFsmHandler.CreateEventWorkflow(eventTypeId, saveChangesToDbContext);

            return _mapper.Map<EventWorkflowDto>(newEventWorkflow);
        }

        public EventWorkflowDto GetWorkflowByEventId(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IList<EmployeeRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<EmployeeRoleDto, EventStatusDto> GetWorkflowApprovalStatusDictById(int eventId)
        {
            throw new System.NotImplementedException();
        }

//        public WorkflowFsmStateInfo WorkflowResponse(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatus)
//        {
//            var systemUser = DatabaseContext.SystemUserRepository.GetSingle(x => x.SystemUserId == respondeeEmployee.SystemUserId);
//            return ValidateAndFireLeaveResponse(EmployeeResponse, employeeEvent, systemUser, (EmployeeRoles)respondeeEmployee.EmployeeRoleId, eventStatus);
//        }

        public WorkflowFsmStateInfo WorkflowResponse(EventDto employeeEvent, int systemUserId, EventStatuses eventStatus)
        {
            var systemUser = DatabaseContext.SystemUserRepository.GetSingle(x => x.SystemUserId == systemUserId);
            var validationActionRole = GetRespondeeValidationActionAndRole(systemUser);
            return ValidateAndFireLeaveResponse(validationActionRole.validationAction, employeeEvent, systemUser, validationActionRole.employeeRole, eventStatus);
        }

        private WorkflowFsmStateInfo ValidateAndFireLeaveResponse(Action<int, EventDto, EventWorkflow, EventStatuses> validationAction, EventDto employeeEvent,
            SystemUser respondeeSystemUser, EmployeeRoles employeeRoles, EventStatuses eventStatus)
        {
            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(
                x => x.EventWorkflowId == employeeEvent.EventWorkflowId,
                x => x.EventWorkflowApprovalResponses);

            validationAction(respondeeSystemUser.SystemUserId, employeeEvent, eventWorkflow, eventStatus);

            return _workflowFsmHandler.FireLeaveResponse(employeeEvent, respondeeSystemUser, employeeRoles, eventStatus, eventWorkflow);
        }

        private (EmployeeRoles employeeRole, Action<int, EventDto, EventWorkflow, EventStatuses> validationAction) GetRespondeeValidationActionAndRole(SystemUser respondeeSystemUser)
        {
            switch ((SystemUserTypes)respondeeSystemUser.SystemUserTypeId)
            {
                case SystemUserTypes.Employee:
                    var employeeRole = (EmployeeRoles)DatabaseContext.EmployeeRepository.GetSingle(x => x.SystemUserId == respondeeSystemUser.SystemUserId).EmployeeRoleId;
                    return (employeeRole, EmployeeResponse);
                case SystemUserTypes.Client:
                    return (EmployeeRoles.Client, ClientResponse);
                default:
                    throw new InvalidOperationException($"No SystemUserType enum exists for integer: {respondeeSystemUser.SystemUserTypeId}");
            }
        }

        private void EmployeeResponse(int systemUserId, EventDto leaveEvent, EventWorkflow eventWorkflow, EventStatuses eventStatus)
        {
            var respondeeEmployee = DatabaseContext.EmployeeRepository.GetSingle(
                x => x.SystemUserId == systemUserId);

            if (eventWorkflow == null)
            {
                throw new ValidationException($"EventWorkflow with id {leaveEvent.EventWorkflowId} does not exist.");
            }

            var requiredResponders = DatabaseContext.EventTypeRequiredRespondersRepository.Get(x => x.EventTypeId == leaveEvent.EventTypeId)
                .Select(x => x.EmployeeRoleId);

            eventWorkflow.EventWorkflowApprovalResponses = DatabaseContext.EmployeeApprovalResponsesRepository.Get(
                x => x.EventWorkflowId == eventWorkflow.EventWorkflowId);

            if (respondeeEmployee.EmployeeId == leaveEvent.EmployeeId && eventStatus == EventStatuses.Cancelled)
            {
                return;
            }
            else if (respondeeEmployee.EmployeeId != leaveEvent.EmployeeId && eventStatus != EventStatuses.Cancelled)
            {
                if (requiredResponders.Contains(respondeeEmployee.EmployeeRoleId) || (EmployeeRoles)respondeeEmployee.EmployeeRoleId == EmployeeRoles.SystemAdministrator)
                {
                    return;
                }
            }
            throw new ValidationException(
                $"Current user does not have the required role or employee id is incorrect to send workflow response.{Environment.NewLine}" +
                $"Employee Role: {respondeeEmployee.EmployeeRoleId}{Environment.NewLine}" +
                $"Required Roles: {string.Join(", ", requiredResponders)}{Environment.NewLine}" +
                $"Event Type: {leaveEvent.EventTypeId}{Environment.NewLine}" +
                $"Employee Id: {respondeeEmployee.EmployeeId}{Environment.NewLine}" +
                $"Event Employee Id: {leaveEvent.EmployeeId}");
        }

        private void ClientResponse(int systemUserId, EventDto leaveEvent, EventWorkflow eventWorkflow, EventStatuses eventStatus)
        {

        }
    }
}
