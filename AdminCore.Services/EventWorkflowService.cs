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
using AdminCore.DTOs.SystemUser;
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

        public IList<SystemUserRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<SystemUserRoleDto, EventStatusDto> GetWorkflowApprovalStatusDictById(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public WorkflowFsmStateInfo WorkflowResponse(EventDto employeeEvent, int systemUserId, EventStatuses eventStatus)
        {
            var systemUser = DatabaseContext.SystemUserRepository.GetSingle(x => x.SystemUserId == systemUserId);
            var validationAction = GetRespondeeValidationActionAndRole(systemUser);
            return ValidateAndFireLeaveResponse(validationAction, employeeEvent, systemUser, (SystemUserRoles)systemUser.SystemUserRoleId, eventStatus);
        }

        private WorkflowFsmStateInfo ValidateAndFireLeaveResponse(Action<SystemUser, EventDto, EventWorkflow, EventStatuses> validationAction, EventDto employeeEvent,
            SystemUser respondeeSystemUser, SystemUserRoles systemUserRoles, EventStatuses eventStatus)
        {
            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(
                x => x.EventWorkflowId == employeeEvent.EventWorkflowId,
                x => x.EventWorkflowApprovalResponses);

            validationAction(respondeeSystemUser, employeeEvent, eventWorkflow, eventStatus);

            return _workflowFsmHandler.FireLeaveResponse(employeeEvent, respondeeSystemUser, systemUserRoles, eventStatus, eventWorkflow);
        }

        private Action<SystemUser, EventDto, EventWorkflow, EventStatuses> GetRespondeeValidationActionAndRole(SystemUser respondeeSystemUser)
        {
            switch ((SystemUserRoles)respondeeSystemUser.SystemUserRoleId)
            {
                case SystemUserRoles.Client:
                    return ClientResponseValidation;
                default:
                    return EmployeeResponseValidation;
            }
        }

        private void EmployeeResponseValidation(SystemUser systemUser, EventDto leaveEvent, EventWorkflow eventWorkflow, EventStatuses eventStatus)
        {
            if (eventWorkflow == null)
            {
                throw new ValidationException($"EventWorkflow with id {leaveEvent.EventWorkflowId} does not exist.");
            }

            var respondeeEmployee = DatabaseContext.EmployeeRepository.GetSingle(
                x => x.SystemUserId == systemUser.SystemUserId);

            var eventTeamContract = DatabaseContext.ContractRepository.GetSingle(
                contract => contract.TeamId == leaveEvent.TeamId)
                                    ?? throw new ValidationException($"Employee {respondeeEmployee.EmployeeId} is not part of Team {leaveEvent.TeamId}.");

            var requiredResponders = DatabaseContext.EventTypeRequiredRespondersRepository.Get(x => x.EventTypeId == leaveEvent.EventTypeId)
                .Select(x => x.EmployeeRoleId);

            eventWorkflow.EventWorkflowApprovalResponses = DatabaseContext.SystemUserApprovalResponsesRepository.Get(
                x => x.EventWorkflowId == eventWorkflow.EventWorkflowId);

            if (respondeeEmployee.EmployeeId == leaveEvent.EmployeeId && eventStatus == EventStatuses.Cancelled)
            {
                return;
            }
            else if (respondeeEmployee.EmployeeId != leaveEvent.EmployeeId && eventStatus != EventStatuses.Cancelled)
            {
                if (requiredResponders.Contains(eventTeamContract.EmployeeId))
                {
                    return;
                }
            }
            throw new ValidationException(
                $"Current user does not have the required role or employee id is incorrect to send workflow response.{Environment.NewLine}" +
                $"System User Role: {systemUser.SystemUserRoleId}{Environment.NewLine}" +
                $"Required Roles: {string.Join(", ", requiredResponders)}{Environment.NewLine}" +
                $"Event Type: {leaveEvent.EventTypeId}{Environment.NewLine}" +
                $"Employee Id: {respondeeEmployee.EmployeeId}{Environment.NewLine}" +
                $"Event Employee Id: {leaveEvent.EmployeeId}");
        }

        private void ClientResponseValidation(SystemUser systemUser, EventDto leaveEvent, EventWorkflow eventWorkflow, EventStatuses eventStatus)
        {
            // Validation to check if client is related to employee who booked this event
            if (eventWorkflow == null)
            {
                throw new ValidationException($"EventWorkflow with id {leaveEvent.EventWorkflowId} does not exist.");
            }

            var respondeeClient = DatabaseContext.ClientRepository.GetSingle(
                x => x.SystemUserId == systemUser.SystemUserId);

            var teamProject = DatabaseContext.TeamRepository.GetSingle(team => team.TeamId == leaveEvent.TeamId,
                team => team.Project);

            if (teamProject.Project.ClientId != respondeeClient.ClientId)
            {
                throw new ValidationException($"Client {leaveEvent.EventWorkflowId} does not exist.");
            }
        }
    }
}
