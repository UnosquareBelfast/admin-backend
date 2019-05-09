using System.Collections;
using System.Collections.Generic;
using AdminCore.Constants.Enums;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Employee;

namespace AdminCore.Services.Tests.ClassData
{
    public class EventWorkflowServiceClassData
    {       
        public class WorkflowResponse_InvalidUserRoleForEventType_ThrowsValidationException : IEnumerable<object[]>
        {
            // int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList
            public IEnumerator<object[]> GetEnumerator()
            {
                // Responding employee is User, approving event required SystemAdmin.
                yield return new object[] { TestClassBuilder.AnnualLeaveEventType().EventTypeId, 
                    new EmployeeDto
                    {
                        EmployeeId = 1,
                        EmployeeRoleId = (int) EmployeeRoles.User
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator}
                    }
                };
                // Responding employee is User, approving event required SystemAdmin and CSE.
                yield return new object[] { TestClassBuilder.AnnualLeaveEventType().EventTypeId, 
                    new EmployeeDto
                    {
                        EmployeeId = 1,
                        EmployeeRoleId = (int) EmployeeRoles.User
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator},
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.Cse}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        
        public class WorkflowResponse_ValidUserRoleForEventType_CallMadeToFsmHandler : IEnumerable<object[]>
        {
            // int eventTypeId, EmployeeDto employeeDto, IList<EventTypeRequiredResponders> eventTypeRequiredRespondersList
            public IEnumerator<object[]> GetEnumerator()
            {
                // Responding employee is SystemAdmin, approving event required SystemAdmin.
                yield return new object[] { TestClassBuilder.AnnualLeaveEventType().EventTypeId, 
                    new EmployeeDto
                    {
                        EmployeeId = 1,
                        EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator}
                    }
                };
                // Responding employee is Teamlead, approving event required SystemAdmin, TeamLeader and Cse.
                yield return new object[] { TestClassBuilder.AnnualLeaveEventType().EventTypeId, 
                    new EmployeeDto
                    {
                        EmployeeId = 1,
                        EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator
                    },
                    new List<EventTypeRequiredResponders>
                    {
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.SystemAdministrator},
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.TeamLeader},
                        new EventTypeRequiredResponders {EventTypeId = TestClassBuilder.AnnualLeaveEventType().EventTypeId, EmployeeRoleId = (int) EmployeeRoles.Cse}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}