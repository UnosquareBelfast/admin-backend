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
        private readonly IFsmWorkflowHandler _fsmWorkflowHandler;
        
        public EventWorkflowService(IDatabaseContext databaseContext, IMapper mapper, IFsmWorkflowHandler fsmWorkflowHandler) : base(databaseContext)
        {
            _mapper = mapper;
            _fsmWorkflowHandler = fsmWorkflowHandler;
        }

        public EventWorkflowDto CreateEventWorkflow(int eventTypeId, bool saveChangesToDbContext = true)
        {
            var newEventWorkflow = _fsmWorkflowHandler.CreateEventWorkflow(eventTypeId, saveChangesToDbContext);

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
//            var eventWorkflow = DatabaseContext.EventTypeRepository.GetSingleThenIncludes(
//                x => x.EventTypeId == employeeEvent.EventTypeId,
//                (x => x.EventTypeRequiredResponders,  new Expression<Func<object, object>>[]
//                {
//                    x => ((EventTypeRequiredResponders)x).EmployeeRole 
//                }));

            var requiredResponders = DatabaseContext.EventTypeRequiredRespondersRepository.Get(x => x.EventTypeId == leaveEvent.EventTypeId)
                .Select(x => x.EmployeeRoleId);

            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(
                x => x.EventWorkflowId == leaveEvent.EventWorkflowId,
                x => x.EventWorkflowApprovalResponses);

            if (eventWorkflow == null)
            {
                throw new ValidationException("Event does not contain reference to event workflow.");
            }
            
            eventWorkflow.EventWorkflowApprovalResponses = DatabaseContext.EmployeeApprovalResponsesRepository.Get(
                x => x.EventWorkflowId == eventWorkflow.EventWorkflowId);
            
            if (requiredResponders.Contains(respondeeEmployee.EmployeeRoleId) || (EmployeeRoles)respondeeEmployee.EmployeeRoleId == EmployeeRoles.SystemAdministrator
                || respondeeEmployee.EmployeeId == leaveEvent.EmployeeId && eventStatuses == EventStatuses.Cancelled)
            {
                return _fsmWorkflowHandler.FireLeaveResponse(leaveEvent, respondeeEmployee, eventStatuses, eventWorkflow); 
                
            }
            throw new ValidationException("Current user does not have the required role to send an approval response on this event");
        }
    }
}