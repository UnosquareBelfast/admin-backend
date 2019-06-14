using System.Collections;
using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.Services.Tests.ClassData
{
    public class EventWorkflowServiceClassData
    {
        public class WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException : IEnumerable<object[]>
        {
            // int systemUserId, EventDto eventDto, EventStatuses eventStatuses,
            // IList<SystemUser> systemUsersDbReturns, IList<Employee> employeeDbReturns, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersDbReturns,
            // IList<EventWorkflow> eventWorkflowDbReturns, IList<SystemUserApprovalResponse> systemUserApprovalResponseDbReturns
            public IEnumerator<object[]> GetEnumerator()
            {
                var systemUserId = 5;

                var employeeEvent = new EventDto
                {
                    EventId = 1,
                    EventWorkflowId = 1
                };

                var eventWorkflow = new EventWorkflow
                {
                    EventWorkflowId = 1
                };

                var systemUserUser = new SystemUser
                {
                    SystemUserId = systemUserId,
                    SystemUserRoleId = (int) SystemUserRoles.User
                };

                var systemUserTeamLead = new SystemUser
                {
                    SystemUserId = systemUserId,
                    SystemUserRoleId = (int) SystemUserRoles.TeamLeader
                };

                // Responding employee is user, approving event required SystemAdmin.
                yield return new object[]
                {
                    systemUserId,
                    employeeEvent,
                    EventStatuses.Approved,
                    new List<SystemUser>
                    {
                        systemUserUser
                    },
                    new List<Employee>
                    {
                        new Employee
                        {
                            EmployeeId = 1,
                            SystemUserId = systemUserId,
                            SystemUser = systemUserUser
                        }
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders
                        {
                            EventTypeId = (int) EventTypes.AnnualLeave,
                            SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator
                        }
                    },
                    new List<EventWorkflow>
                    {
                        eventWorkflow
                    },
                    new List<SystemUserApprovalResponse>()
                };
                // Responding employee is TeamLead, approving event required SystemAdmin and Cse.
                yield return new object[]
                {
                    systemUserId,
                    employeeEvent,
                    EventStatuses.Approved,
                    new List<SystemUser>
                    {
                        systemUserTeamLead
                    },
                    new List<Employee>
                    {
                        new Employee
                        {
                            EmployeeId = 1,
                            SystemUserId = systemUserId,
                            SystemUser = systemUserTeamLead
                        }
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders
                        {
                            EventTypeId = (int) EventTypes.AnnualLeave,
                            SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator
                        },
                        new EventTypeRequiredResponders
                        {
                            EventTypeId = (int) EventTypes.AnnualLeave,
                            SystemUserRoleId = (int) SystemUserRoles.Cse
                        }
                    },
                    new List<EventWorkflow>
                    {
                        eventWorkflow
                    },
                    new List<SystemUserApprovalResponse>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler : IEnumerable<object[]>
        {
            // int systemUserId, EventDto eventDto, EventStatuses eventStatuses,
            // IList<SystemUser> systemUsersDbReturns, IList<Employee> employeeDbReturns, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersDbReturns,
            // IList<EventWorkflow> eventWorkflowDbReturns, IList<SystemUserApprovalResponse> systemUserApprovalResponseDbReturns
            public IEnumerator<object[]> GetEnumerator()
            {
               var systemUserId = 5;

               var employeeEvent = new EventDto
               {
                   EventId = 1,
                   EventWorkflowId = 1
               };

               var eventWorkflow = new EventWorkflow
               {
                   EventWorkflowId = 1
               };

               var systemUserAdmin = new SystemUser
               {
                   SystemUserId = systemUserId,
                   SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator
               };

               var systemUserTeamLead = new SystemUser
               {
                   SystemUserId = systemUserId,
                   SystemUserRoleId = (int) SystemUserRoles.TeamLeader
               };

                // Responding employee is SystemAdmin, approving event required SystemAdmin.
                yield return new object[]
                {
                    systemUserId,
                    employeeEvent,
                    EventStatuses.Approved,
                    new List<SystemUser>
                    {
                        systemUserAdmin
                    },
                    new List<Employee>
                    {
                        new Employee
                        {
                            EmployeeId = 1,
                            SystemUserId = systemUserId,
                            SystemUser = systemUserAdmin
                        }
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders
                        {
                            EventTypeId = (int) EventTypes.AnnualLeave,
                            SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator
                        }
                    },
                    new List<EventWorkflow>
                    {
                        eventWorkflow
                    },
                    new List<SystemUserApprovalResponse>()
                };
                // Responding employee is Teamlead, approving event required SystemAdmin, TeamLeader and Cse.
                yield return new object[]
                {
                    systemUserId,
                    employeeEvent,
                    EventStatuses.Approved,
                    new List<SystemUser>
                    {
                        systemUserTeamLead
                    },
                    new List<Employee>
                    {
                       new Employee
                       {
                           EmployeeId = 1,
                           SystemUserId = systemUserId,
                           SystemUser = systemUserTeamLead
                       }
                    },
                    new List<EventTypeRequiredResponders>
                    {
                       new EventTypeRequiredResponders
                       {
                           EventTypeId = (int) EventTypes.AnnualLeave,
                           SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator
                       },
                       new EventTypeRequiredResponders
                       {
                           EventTypeId = (int) EventTypes.AnnualLeave,
                           SystemUserRoleId = (int) SystemUserRoles.TeamLeader
                       },
                       new EventTypeRequiredResponders
                       {
                           EventTypeId = (int) EventTypes.AnnualLeave,
                           SystemUserRoleId = (int) SystemUserRoles.Cse
                       }
                    },
                    new List<EventWorkflow>
                    {
                       eventWorkflow
                    },
                    new List<SystemUserApprovalResponse>()
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
