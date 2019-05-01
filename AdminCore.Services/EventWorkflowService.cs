using System.Collections.Generic;
using AdminCore.Common.Interfaces;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.EventWorkflow;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AutoMapper;

namespace AdminCore.Services
{
    public class EventWorkflowService : IEventWorkflowService
    {
        private IMapper _mapper;
        public EventWorkflowService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public EventWorkflowDto AddEventWorkflow(int eventId, EmployeeDto employee)
        {
            throw new System.NotImplementedException();
        }

        public EventWorkflowDto AddEventWorkflow(EventDto employeeEvent, EmployeeDto employee)
        {
            throw new System.NotImplementedException();
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

        public EventStatuses UpdateWorkflowResponse(int eventId, EmployeeDto employee, EventStatuses eventStatus)
        {
            throw new System.NotImplementedException();
        }

        public EventStatuses UpdateWorkflowResponse(EventDto employeeEvent, EmployeeDto employee, EventStatuses eventStatus)
        {
            throw new System.NotImplementedException();
        }
    }
}