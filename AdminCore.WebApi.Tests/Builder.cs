using System;
using System.Collections.Generic;
using System.Security.Claims;
using AdminCore.Common;
using AdminCore.Constants;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.DTOs.Event;
using AdminCore.DTOs.SystemUser;
using AdminCore.WebApi.Models.Employee;
using AdminCore.WebApi.Models.SystemUser;

namespace AdminCore.WebApi.Tests
{
  public class Builder
  {
    public static IList<EventDto> BuildListOfEvents(int numberOfEvents, DateTime dateCreated, int employeeId)
    {
      var listOfEvents = new List<EventDto>();
      for (var i = 1; i <= numberOfEvents; i++)
      {
        listOfEvents.Add(BuildTestEvent(i, dateCreated, employeeId));
      }

      return listOfEvents;
    }

    public static EventDto BuildTestEvent(int eventId, DateTime dateCreated, int employeeId)
    {
      return new EventDto
      {
        EventId = eventId,
        DateCreated = dateCreated,
        EmployeeId = employeeId
      };
    }

    public static UpdateEmployeeViewModel BuildUpdateEmployeeViewModel(int employeeId)
    {
      return new UpdateEmployeeViewModel
      {
        EmployeeId = employeeId,
        Forename = "Test",
        Surname = "Employee",
        Email = "test@employee.com",
        CountryId = 1,
        SystemUser = new CreateSystemUserViewModel
        {
          SystemUserRoleId = (int) SystemUserRoles.TeamLeader
        },
        EmployeeStatusId = 1,
        StartDate = new DateTime()
      };
    }

    public static EmployeeDto BuildTestEmployee(int employeeId)
    {
      return new EmployeeDto
      {
        EmployeeId = employeeId,
        Forename = "Test",
        Surname = "Employee",
        Email = "test@employee.com",
        CountryId = 1,
        SystemUser = new SystemUserDto
        {
          SystemUserRoleId = (int) SystemUserRoles.TeamLeader
        },
        EmployeeStatusId = 1,
        StartDate = new DateTime()
      };
    }

    public static RegisterEmployeeViewModel BuildRegisterEmployeeViewModel(DateTime startDate)
    {
      return new RegisterEmployeeViewModel
      {
        CountryId = 1,
        SystemUser = new CreateSystemUserViewModel
        {
          SystemUserRoleId = (int) SystemUserRoles.User
        },
        EmployeeStatusId = 1,
        StartDate = startDate
      };
    }

    public static UserDetailsHelper BuildUserDetails()
    {
      var claims = new List<Claim>
      {
        new Claim(UserDetailsConstants.UserEmail, "test@employee.com"),
        new Claim(UserDetailsConstants.Name, "Test Employee"),
      };
      return new UserDetailsHelper(claims);
    }
  }
}
