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
using Microsoft.EntityFrameworkCore;

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
            
            DatabaseContext.EventWorkflowRepository.Insert(newEventWorkflow);
            DatabaseContext.SaveChanges();

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
            var eventWorkflow = DatabaseContext.EventTypeRepository.GetSingle(
                x => x.EventTypeId == employeeEvent.EventId,
                x => x.EventTypeRequiredResponders,
                b => b.Include(c => c.EventTypeRequiredResponders).ThenInclude(c => c.EmployeeRole));

//            var eventWorkflow = DatabaseContext.EventWorkflowRepository.GetSingle(
//                x => x.EventId == employeeEvent.EventId,
//                b => b.Include(c => c.Event)
//                    .Include(c => c.EventWorkflowResponders).ThenInclude(c => c.EmployeeRole)
//                    .Include(c => c.EventWorkflowApprovalStatuses).ThenInclude(c => c.EmployeeRole));

//            return _fsmWorkflowHandler.FireLeaveResponse(employeeEvent, respondeeEmployee, eventStatuses, eventWorkflow);
        }
    }
}