using System;
using System.Net;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Employee;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Exceptions;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Tests.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{

  public class AuthenticationControllerTests : BaseControllerTest
  {
    private readonly AuthenticationController _authenticationController;
    private const int TestEmployeeId = 1;
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IEmployeeService _employeeService;
    private static readonly DateTime TestDate = new DateTime(2019, 01, 01, 0, 0, 0);



    public AuthenticationControllerTests()
    {
      var testEmployee = Builder.BuildTestEmployee(TestEmployeeId);
      _authenticatedUser = Substitute.For<IAuthenticatedUser>();
      _authenticatedUser.RetrieveLoggedInUser().Returns(testEmployee);
      _employeeService = Substitute.For<IEmployeeService>();
      var mapper = new Mapper(new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new WebMappingProfile());
        cfg.AddProfile(new SystemUserMappingProfile());
      }));
      _authenticationController = new AuthenticationController(_authenticatedUser, _employeeService, mapper);
    }

    [Fact]
    public void TestRegisterReturnsOkResultWithErrorStringWhenUserIsAlreadyRegister()
    {
      var result = _authenticationController.Register(Builder.BuildRegisterEmployeeViewModel(TestDate));
      var resultValue = RetrieveValueFromActionResult<string>(result);
      Assert.Equal($"User with email test@employee.com already exists. If you need to change details, use update.", resultValue);
    }

    [Fact]
    public void TestRegisterReturnsOkResultWithSuccessStringWhenUserIsNewlyRegistered()
    {
      var testUserEmail = "test@employee.com";
      _authenticatedUser.When(x => x.RetrieveLoggedInUser()).Throw(new UserNotRegisteredException("Test exception"));
      _authenticatedUser.GetLoggedInUserDetails().Returns(Builder.BuildUserDetails());
      _employeeService.Create(Arg.Any<EmployeeDto>()).Returns(testUserEmail);

      var result = _authenticationController.Register(Builder.BuildRegisterEmployeeViewModel(TestDate));
      var resultValue = RetrieveValueFromActionResult<string>(result);

      Assert.Equal($"User with email address {testUserEmail} successfully registered.", resultValue);
    }

    [Fact]
    public void TestRegisterReturnsError500ResultWithErrorStringWhenRegisteringNewUserFails()
    {
      var testUserEmail = "test@employee.com";
      var testException = new TestException("Test exception");
      _authenticatedUser.When(x => x.RetrieveLoggedInUser()).Throw(new UserNotRegisteredException("User not registered"));
      _authenticatedUser.GetLoggedInUserDetails().Returns(Builder.BuildUserDetails());
      _employeeService.When(x => x.Create(Arg.Any<EmployeeDto>())).Throw(new TestException("Test exception"));

      var result = _authenticationController.Register(Builder.BuildRegisterEmployeeViewModel(TestDate));
      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);

      Assert.Equal($"An error has occurred while registering new employee {testUserEmail}: {testException.Message}", resultValue);
    }

    [Fact]
    public void TestCheckAuthenticatedUserExistsReturnsOkResultWhenUserExists()
    {
      var result = _authenticationController.CheckAuthenticatedUserExists();
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void TestCheckAuthenticatedUserExistsReturnsNoContentResultWhenUserDoesNotExist()
    {
      _authenticatedUser.When(x => x.RetrieveLoggedInUser()).Throw(new UserNotRegisteredException("User not registered"));
      var result = _authenticationController.CheckAuthenticatedUserExists();
      Assert.IsType<NoContentResult>(result);
    }
  }
}
