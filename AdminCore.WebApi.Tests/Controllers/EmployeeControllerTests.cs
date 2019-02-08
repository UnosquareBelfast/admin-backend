using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Employee;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Models.Employee;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class EmployeeControllerTests : BaseControllerTest
  {
    private readonly IEmployeeService _employeeService;
    private readonly EmployeeController _employeeController;
    private readonly IFixture _fixture;
    private const int TestEmployeeId = 1;

    public EmployeeControllerTests()
    {
      _employeeService = Substitute.For<IEmployeeService>();
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new WebMappingProfile())));
      _fixture = new Fixture();
      var authenticatedUser = Substitute.For<IAuthenticatedUser>();
      authenticatedUser.RetrieveLoggedInUser().Returns(Builder.BuildTestEmployee(TestEmployeeId));
      _employeeController = new EmployeeController(_employeeService, mapper, authenticatedUser);
    }

    [Fact]
    public void GetAllEmployee_WhenCalled_ReturnsAllEmployees()
    {
      // Arrange
      const int numberOfEmployees = 9;

      var employeeDtos = _fixture.CreateMany<EmployeeDto>(numberOfEmployees).ToList();

      _employeeService.GetAll().Returns(employeeDtos);

      

      // Act
      var result = _employeeController.GetAllEmployees();

      // Assert
      var resultValue = RetrieveValueFromActionResult<List<EmployeeViewModel>>(result);
      Assert.Equal(resultValue.Count(), numberOfEmployees);
    }

    [Fact]
    public void GetAllEmployeeReturnsErrorMsgWhenNoEmployeesInDb()
    {
      // Service returns empty list.
      _employeeService.GetAll().Returns(new List<EmployeeDto>());

      // Act
      var result = _employeeController.GetAllEmployees();

      // Assert
      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("No employees currently exist.", resultValue);
    }

    [Fact]
    public void TestUpdateEmployeeReturnsEmptyOkResponseWhenGivenValidInput()
    {
      var updateViewModel = Builder.BuildUpdateEmployeeViewModel(TestEmployeeId);

      var result = _employeeController.UpdateEmployee(updateViewModel);

      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void TestUpdateEmployeeReturnsOkResponseWithErrorMessageWhenSaveThrowsAnException()
    {
      var updateViewModel = Builder.BuildUpdateEmployeeViewModel(TestEmployeeId);

      _employeeService.When(x => x.Save(Arg.Any<EmployeeDto>())).Throw(new Exception("Test Exception"));

      var result = _employeeController.UpdateEmployee(updateViewModel);

      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("Something went wrong, employee was not updated.", resultValue);
    }

    [Fact]
    public void TestGetEmployeeByIdReturnsOkObjectResultWithViewModelWhenGivenValidId()
    {
      var employeeDtoReturnedFromService = Builder.BuildTestEmployee(TestEmployeeId);
      
      _employeeService.Get(TestEmployeeId).Returns(employeeDtoReturnedFromService);

      var result = _employeeController.GetEmployeeById(TestEmployeeId);

      RetrieveValueFromActionResult<EmployeeViewModel>(result);
    }

    [Fact]
    public void TestGetEmployeeByIdReturnsOkObjectResultWithErrorMsgWhenGivenInvalidId()
    {
      _employeeService.Get(TestEmployeeId).ReturnsNull();

      var result = _employeeController.GetEmployeeById(TestEmployeeId);

      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("No employee found with an ID of 1", resultValue);
    }

    [Fact]
    public void TestGetEmployeeByEmployeeNameReturnsOkObjectResultWithEmployeeViewModelWhenEmployeeNameExists()
    {
      const string testEmployeeForename = "Test";
      const string testEmployeeSurname = "Employee";

      var listOfDtosReturnedFromService = new List<EmployeeDto>()
      {
        Builder.BuildTestEmployee(TestEmployeeId)
      };

      _employeeService.GetByForenameAndSurname(testEmployeeForename, testEmployeeSurname).Returns(listOfDtosReturnedFromService);

      var result = _employeeController.GetByForenameAndSurname(testEmployeeForename, testEmployeeSurname);

      RetrieveValueFromActionResult<IList<EmployeeViewModel>>(result);
    }

    [Fact]
    public void TestGetEmployeeByEmployeeNameReturnsOkObjectResultWithErrorMsgWhenEmployeeNameDoesNotExist()
    {
      const string testEmployeeForename = "Test";
      const string testEmployeeSurname = "Employee";

      var listOfDtosReturnedFromService = new List<EmployeeDto>();
      _employeeService.GetByForenameAndSurname(testEmployeeForename, testEmployeeSurname).Returns(listOfDtosReturnedFromService);

      var result = _employeeController.GetByForenameAndSurname(testEmployeeForename, testEmployeeSurname);

      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("No employee found with a forename of Test and a surname of Employee", resultValue);
    }

    [Fact]
    public void TestGetEmployeeByCountryIdReturnsOkObjectResultWithEmployeeViewModelWhenEmployeeExistsWithThatCountryId()
    {
      var listOfDtosReturnedFromService = new List<EmployeeDto>()
      {
        Builder.BuildTestEmployee(TestEmployeeId)
      };

      _employeeService.GetByCountryId(TestEmployeeId).Returns(listOfDtosReturnedFromService);

      var result = _employeeController.GetByCountryId(TestEmployeeId);

      RetrieveValueFromActionResult<IList<EmployeeViewModel>>(result);
    }

    [Fact]
    public void TestGetEmployeeByCountryIdReturnsOkObjectResultWithErrorMsgWhenEmployeeDoesNotExistWithThatCountryId()
    {
      var listOfDtosReturnedFromService = new List<EmployeeDto>();
      _employeeService.GetByCountryId(TestEmployeeId).Returns(listOfDtosReturnedFromService);

      var result = _employeeController.GetByCountryId(TestEmployeeId);

      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("No employee found with country ID 1", resultValue);
    }

    [Fact]
    public void TestDeleteEmployeeReturnsOkObjectResultWithSuccessMessageWhenEmployeeExists()
    {
      var result = _employeeController.DeleteEmployee(TestEmployeeId);
      var resultValue = RetrieveValueFromActionResult<string>(result);

      Assert.Equal("Employee with Employee ID 1 has been successfully deleted.", resultValue);
    }

    [Fact]
    public void TestDeleteEmployeeReturnsOkObjectResultWithErrorMessageWhenEmployeeDoesNotExist()
    {
      _employeeService.When(x => x.Delete(Arg.Any<int>())).Throw(new Exception("Test Exception"));

      var result = _employeeController.DeleteEmployee(TestEmployeeId);
      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);

      Assert.Equal("Something went wrong, employee was not deleted.", resultValue);
    }

    [Fact]
    public void TestGetSignedInUserReturnsUserDetailsWhenUserIsSignedIn()
    {
      var result = _employeeController.GetSignedInUser();
      var resultValue = RetrieveValueFromActionResult<EmployeeViewModel>(result);
      Assert.Equal(TestEmployeeId, resultValue.EmployeeId);
    }

  }
} 
