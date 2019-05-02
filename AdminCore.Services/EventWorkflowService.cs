using System.Collections.Generic;
using System.Linq;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DAL;
using AdminCore.DAL.Models;
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
            return _mapper.Map<EventWorkflowDto>(_fsmWorkflowHandler.CreateEventWorkflow(eventId, eventTypeId));
        }
        
        public EventWorkflowDto GetWorkflowByEventId(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IList<EmployeeRoleDto> GetWorkflowApproversEmployeeRoleListById(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<EmployeeRoleDto, ApprovalStatusDto> GetWorkflowApprovalStatusDictById(int eventId)
        {
            throw new System.NotImplementedException();
        }

        public bool WorkflowResponseApprove(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Approved);
        }

        public bool WorkflowResponseReject(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Rejected);
        }

        public bool WorkflowResponseCancel(EventDto employeeEvent, EmployeeDto respondeeEmployee)
        {
            return FireWorkflowTrigger(employeeEvent, respondeeEmployee, EventStatuses.Cancelled);
        }

        private bool FireWorkflowTrigger(EventDto employeeEvent, EmployeeDto respondeeEmployee, EventStatuses eventStatuses)
        {
            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(x => x.EventId == employeeEvent.EventId);
            return _fsmWorkflowHandler.FireLeaveResponse(employeeEvent, respondeeEmployee, eventStatuses, eventWorkflow);
        }
    }
}