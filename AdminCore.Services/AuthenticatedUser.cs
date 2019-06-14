using System.Security.Authentication;
using AdminCore.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AdminCore.Common;
using AdminCore.Constants;
using AdminCore.Constants.Enums;
using AdminCore.DTOs.Employee;
using AdminCore.WebApi.Exceptions;

namespace AdminCore.Services
{
  public class AuthenticatedUser : IAuthenticatedUser
  {
    private readonly IHttpContextAccessor _httpContentAccessor;
    private readonly IEmployeeService _employeeService;

    private const string Admin = "Admin";

    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor, IEmployeeService employeeService)
    {
      _httpContentAccessor = httpContextAccessor;
      _employeeService = employeeService;
    }

    public EmployeeDto RetrieveLoggedInUser()
    {
      var userDetails = GetLoggedInUserDetails();
      var employee = _employeeService.GetEmployeeByEmail(userDetails[UserDetailsConstants.UserEmail]);
//      GetRoleFromAzure(employee, userDetails);
      return employee ?? throw new UserNotRegisteredException($"User with email {userDetails[UserDetailsConstants.UserEmail]} is not registered. Log in first.");
    }

    private static void GetRoleFromAzure(EmployeeDto employee, UserDetailsHelper userDetails)
    {
      if (employee != null)
      {
        AddRoleToEmployee(employee, userDetails);
      }
    }

    private static void AddRoleToEmployee(EmployeeDto employee, UserDetailsHelper userDetails)
    {
      if (UserIsAdmin(userDetails))
      {
        employee.SystemUser.SystemUserRoleId = (int) SystemUserRoles.SystemAdministrator;
      }
      else
      {
        employee.SystemUser.SystemUserRoleId = (int) SystemUserRoles.User;
      }
    }

    private static bool UserIsAdmin(UserDetailsHelper userDetails)
    {
      return userDetails.ContainsKey(UserDetailsConstants.Role) && userDetails[UserDetailsConstants.Role].Equals(Admin);
    }

    public UserDetailsHelper GetLoggedInUserDetails()
    {
      var identity = _httpContentAccessor.HttpContext.User.Identity as ClaimsIdentity;
      var userDetails = new UserDetailsHelper(identity.Claims);
      return userDetails.Count > 0 ? userDetails : throw new AuthenticationException("No user is currently authenticated");
    }
  }

}
