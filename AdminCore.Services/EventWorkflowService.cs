using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.Common;
using AdminCore.Common.Exceptions;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
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

        public WorkflowFsmStateInfo WorkflowResponseApprove(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Approved);
        }

        public WorkflowFsmStateInfo WorkflowResponseReject(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Rejected);
        }

        public WorkflowFsmStateInfo WorkflowResponseCancel(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Cancelled);
        }

        private WorkflowFsmStateInfo FireWorkflowTrigger(EventDto leaveEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatuses)
        {
            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(
                x => x.EventWorkflowId == leaveEvent.EventWorkflowId,
                x => x.EventWorkflowApprovalResponses);

            if (eventWorkflow == null)
            {
                throw new ValidationException($"EventWorkflow with id {leaveEvent.EventWorkflowId} does not exist.");
            }

            var requiredResponders = DatabaseContext.EventTypeRequiredRespondersRepository.Get(x => x.EventTypeId == leaveEvent.EventTypeId)
                .Select(x => x.EmployeeRoleId);
            
            eventWorkflow.EventWorkflowApprovalResponses = DatabaseContext.EmployeeApprovalResponsesRepository.Get(
                x => x.EventWorkflowId == eventWorkflow.EventWorkflowId);

            if (respondeeEmployee.EmployeeId == leaveEvent.EmployeeId && eventStatuses == EventStatuses.Cancelled)
            {
                return _workflowFsmHandler.FireLeaveResponse(leaveEvent, respondeeEmployee, eventStatuses, eventWorkflow); 
            }
            else if (respondeeEmployee.EmployeeId != leaveEvent.EmployeeId && eventStatuses != EventStatuses.Cancelled)
            {
                if (requiredResponders.Contains(respondeeEmployee.EmployeeRoleId) || (EmployeeRoles)respondeeEmployee.EmployeeRoleId == EmployeeRoles.SystemAdministrator)
                {
                    return _workflowFsmHandler.FireLeaveResponse(leaveEvent, respondeeEmployee, eventStatuses, eventWorkflow); 
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
    }
}
