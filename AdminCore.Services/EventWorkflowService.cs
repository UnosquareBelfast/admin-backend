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
        private readonly IEmployeeService _employeeService;
        private readonly IFsmWorkflowHandler _fsmWorkflowHandler;
        
        public EventWorkflowService(IDatabaseContext databaseContext, IMapper mapper, IEmployeeService employeeService, IFsmWorkflowHandler fsmWorkflowHandler) : base(databaseContext)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _fsmWorkflowHandler = fsmWorkflowHandler;
        }

        public EventWorkflowDto CreateEventWorkflow(int eventId, int eventTypeId)
        {
            var newEventWorkflow = _fsmWorkflowHandler.CreateEventWorkflow(eventId, eventTypeId);

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

        private WorkflowFsmStateInfo FireWorkflowTrigger(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatuses)
        {
//            var eventWorkflow = DatabaseContext.EventTypeRepository.GetSingleThenIncludes(
//                x => x.EventTypeId == employeeEvent.EventTypeId,
//                (x => x.EventTypeRequiredResponders,  new Expression<Func<object, object>>[]
//                {
//                    x => ((EventTypeRequiredResponders)x).EmployeeRole 
//                }));

            var requiredResponders = DatabaseContext.EventTypeRequiredRespondersRepository.Get(x => x.EventTypeId == employeeEvent.EventTypeId)
                .Select(x => x.EmployeeRoleId);

            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(x => x.EventId == employeeEvent.EventId, 
                null,
                x => x.EventWorkflowApprovalResponses);

            eventWorkflow.EventWorkflowApprovalResponses = DatabaseContext.EmployeeApprovalResponsesRepository.Get(
                x => x.EventWorkflowId == eventWorkflow.EventWorkflowId);
            
            if (!requiredResponders.Contains(respondeeEmployee.EmployeeRoleId) && (EmployeeRoles)respondeeEmployee.EmployeeRoleId != EmployeeRoles.SystemAdministrator)
            {
                throw new ValidationException("Current user does not have the required role to send an approval response on this event");
            }
           
            return _fsmWorkflowHandler.FireLeaveResponse(employeeEvent, respondeeEmployee, eventStatuses, eventWorkflow); 
        }
    }
}