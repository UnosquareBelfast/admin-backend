using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdminCore.Common.Interfaces;
using AdminCore.DTOs;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Controllers;
using AdminCore.WebApi.Mappings;
using AdminCore.WebApi.Models.Contract;
using AdminCore.WebApi.Models.Team;
using AdminCore.WebApi.Tests.ClassData;
using AdminCore.WebApi.Tests.Exceptions;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.WebApi.Tests.Controllers
{
  public class ContractControllerTests : BaseControllerTest
  {
    private readonly ContractController _contractController;
    private readonly IContractService _contractService;
    private readonly TestException _testException;

    public ContractControllerTests()
    {
      _contractService = Substitute.For<IContractService>();
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new WebMappingProfile())));
      _contractController = new ContractController(mapper, _contractService);
      _testException = new TestException("Test Exception");
    }

    [Fact]
    public void TestGetContractByIdReturnsOkObjectResultWithContractViewModelWhenIdExists()
    {
      const int testId = 1;

      var contractDtoReturnedFromService = BuildContractDto();
      _contractService.GetContractById(testId).Returns(contractDtoReturnedFromService);

      var result = _contractController.GetContractById(testId);
      var resultValue = RetrieveValueFromActionResult<ContractViewModel>(result);
      AssertContractDtoAndViewModelAreIdentical(contractDtoReturnedFromService, resultValue);
      _contractService.Received(1).GetContractById(testId);
    }

    [Fact]
    public void TestGetContractByIdReturnsNoContentResultWhenIdDoesNotExist()
    {
      const int testId = 1;
      _contractService.GetContractById(testId).ReturnsNull();

      var result = _contractController.GetContractById(testId);
      Assert.IsType<NoContentResult>(result);
      _contractService.Received(1).GetContractById(testId);
    }

    [Fact]
    public void TestGetContractByEmployeeIdReturnsOkObjectResultWithContractViewModelListWhenContractsExist()
    {
      const int testId = 1;

      var contractDtoReturnedFromService = BuildContractDto();
      var contractDtoList = new List<ContractDto>()
      {
        contractDtoReturnedFromService
      };

      _contractService.GetContractByEmployeeId(testId).Returns(contractDtoList);

      var result = _contractController.GetContractByEmployeeId(testId);
      var resultValue = RetrieveValueFromActionResult<IList<ContractViewModel>>(result);
      AssertContractDtoAndViewModelAreIdentical(contractDtoReturnedFromService, resultValue.First());
      _contractService.Received(1).GetContractByEmployeeId(testId);
    }

    [Fact]
    public void TestGetContractByEmployeeIdReturnsNoContentResultWhenNoContractsExist()
    {
      const int testId = 1;
      _contractService.GetContractByEmployeeId(testId).Returns(new List<ContractDto>());

      var result = _contractController.GetContractByEmployeeId(testId);
      Assert.IsType<NoContentResult>(result);
      _contractService.Received(1).GetContractByEmployeeId(testId);
    }

    [Fact]
    public void TestGetContractByTeamIdReturnsOkObjectResultWithContractViewModelListWhenContractsExist()
    {
      const int testId = 1;

      var contractDtoReturnedFromService = BuildContractDto();
      var contractDtoList = new List<ContractDto>()
      {
        contractDtoReturnedFromService
      };

      _contractService.GetContractByTeamId(testId).Returns(contractDtoList);

      var result = _contractController.GetContractByTeamId(testId);
      var resultValue = RetrieveValueFromActionResult<IList<ContractViewModel>>(result);
      AssertContractDtoAndViewModelAreIdentical(contractDtoReturnedFromService, resultValue.First());
      _contractService.Received(1).GetContractByTeamId(testId);
    }

    [Fact]
    public void TestGetContractByTeamIdReturnsNoContentResultWhenNoContractsExist()
    {
      const int testId = 1;
      _contractService.GetContractByTeamId(testId).Returns(new List<ContractDto>());

      var result = _contractController.GetContractByTeamId(testId);
      Assert.IsType<NoContentResult>(result);
      _contractService.Received(1).GetContractByTeamId(testId);
    }

    [Fact]
    public void TestGetContractByEmployeeIdAndTeamIdReturnsOkObjectResultWithContractViewModelListWhenContractsExist()
    {
      const int testId = 1;

      var contractDtoReturnedFromService = BuildContractDto();
      var contractDtoList = new List<ContractDto>()
      {
        contractDtoReturnedFromService
      };

      _contractService.GetContractByEmployeeIdAndTeamId(testId, testId).Returns(contractDtoList);

      var result = _contractController.GetContractByEmployeeIdAndTeamId(testId, testId);
      var resultValue = RetrieveValueFromActionResult<IList<ContractViewModel>>(result);
      AssertContractDtoAndViewModelAreIdentical(contractDtoReturnedFromService, resultValue.First());
      _contractService.Received(1).GetContractByEmployeeIdAndTeamId(testId, testId);
    }

    [Fact]
    public void TestGetContractByEmployeeIdAndTeamIdReturnsNoContentResultWhenNoContractsExist()
    {
      const int testId = 1;
      _contractService.GetContractByEmployeeIdAndTeamId(testId, testId).Returns(new List<ContractDto>());

      var result = _contractController.GetContractByEmployeeIdAndTeamId(testId, testId);
      Assert.IsType<NoContentResult>(result);
      _contractService.Received(1).GetContractByEmployeeIdAndTeamId(testId, testId);
    }

    [Theory]
    [ClassData(typeof(ContractControllerClassData.ProjectIdFixtureListDataGetContractByProjectId))]
    public void GetContractByProjectId_ServiceReturnsListOfContracts_ReturnsOkWithContractsInBody(int projectId, IList<ContractDto> serviceReturns, int expectedReturnCount)
    {
      // Arrange
      GetMockedResourcesGetContractByProjectId(projectId, serviceReturns, out var contractServiceMock, out var contractController, out var mapper);

      // Act
      var response = contractController.GetContractByProjectId(projectId);

      // Assert
      response.Should().BeOfType<OkObjectResult>();
      contractServiceMock.Received(1).GetContractByProjectId(Arg.Any<int>());
      mapper.Received(1).Map<IList<ContractViewModel>>(Arg.Is<IList<ContractDto>>(x => x != null && x.Count == expectedReturnCount));
    }

    [Fact]
    public void GetContractByProjectId_ServiceReturnsEmptyListOfContracts_ReturnsNoContent()
    {
      // Arrange
      GetMockedResourcesGetContractByProjectId(600, new List<ContractDto>(), out var contractServiceMock, out var contractController, out var mapper);

      // Act
      var response = contractController.GetContractByProjectId(600);

      // Assert
      response.Should().BeOfType<NoContentResult>();
      contractServiceMock.Received(1).GetContractByProjectId(Arg.Any<int>());
    }


    [Fact]
    public void GetContractByProjectId_ServiceReturnsNullListOfContracts_ReturnsNoContent()
    {
      // Arrange
      GetMockedResourcesGetContractByProjectId(3123, null, out var contractServiceMock, out var contractController, out var mapper);

      // Act
      var response = contractController.GetContractByProjectId(3123);

      // Assert
      response.Should().BeOfType<NoContentResult>();
      contractServiceMock.Received(1).GetContractByProjectId(Arg.Any<int>());
    }

    [Fact]
    public void TestCreateContractReturnsOkObjectResultWithSuccessMsgWhenServiceDoesNotThrowException()
    {
      var createContractViewModel = BuildCreateContractViewModel();
      var result = _contractController.CreateContract(createContractViewModel);
      var resultValue = RetrieveValueFromActionResult<string>(result);
      Assert.Equal("Contract successfully created.", resultValue);
    }

    [Fact]
    public void TestCreateContractReturnsStatusCode500ResultWhenServiceThrowsException()
    {
      var createContractViewModel = BuildCreateContractViewModel();
      _contractService.When(x => x.SaveContract(Arg.Any<ContractDto>())).Throw(_testException);
      var result = _contractController.CreateContract(createContractViewModel);
      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal($"Something went wrong. Contract was not created: {_testException.Message}", resultValue);
    }

    [Fact]
    public void TestUpdateContractReturnsOkObjectResultWithSuccessMsgWhenServiceDoesNotThrowException()
    {
      var createContractViewModel = BuildUpdateContractViewModel();
      var result = _contractController.UpdateContract(createContractViewModel);
      var resultValue = RetrieveValueFromActionResult<string>(result);
      Assert.Equal("Contract successfully updated.", resultValue);
    }

    [Fact]
    public void TestUpdateContractReturnsStatusCode500ResultWhenServiceThrowsException()
    {
      var createContractViewModel = BuildUpdateContractViewModel();
      _contractService.When(x => x.SaveContract(Arg.Any<ContractDto>())).Throw(new Exception("Test Exception"));
      var result = _contractController.UpdateContract(createContractViewModel);
      var resultValue = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("Something went wrong. Contract was not updated.", resultValue);
    }

    [Fact]
    public void TestDeleteContractReturnsOkObjectResultWithSuccessMsgWhenServiceDoesNotThrowException()
    {
      const int testId = 1;
      var result = _contractController.DeleteContract(testId);
      var successMsg = RetrieveValueFromActionResult<string>(result);
      Assert.Equal("Contract successfully deleted.", successMsg);
    }

    [Fact]
    public void TestDeleteContractReturnsStatusCode500ResultWithErrorMsgWhenServiceThrowsException()
    {
      const int testId = 1;
      _contractService.When(x => x.DeleteContract(testId)).Throw(new Exception("Test Exception"));
      var result = _contractController.DeleteContract(testId);
      var errorMsg = RetrieveValueFromActionResult<string>(result, HttpStatusCode.InternalServerError);
      Assert.Equal("Something went wrong. Contract was not deleted.", errorMsg);
    }

    private static ContractDto BuildContractDto()
    {
      return new ContractDto()
      {
        ContractId = 1,
        EmployeeId = 1,
        Team = new TeamDto()
        {
          TeamId = 1
        },
        StartDate = new DateTime(2018, 12, 10),
        EndDate = new DateTime(2019, 1, 10)
      };
    }

    private static CreateContractViewModel BuildCreateContractViewModel()
    {
      return new CreateContractViewModel()
      {
        EmployeeId = 1,
        TeamId = 1,
        StartDate = new DateTime(2018, 12, 10),
        EndDate = new DateTime(2019, 1, 10)
      };
    }

    private static UpdateContractViewModel BuildUpdateContractViewModel()
    {
      return new UpdateContractViewModel()
      {
        ContractId = 1,
        EmployeeId = 1,
        TeamId = 1,
        StartDate = new DateTime(2018, 12, 10),
        EndDate = new DateTime(2019, 1, 10)
      };
    }

    private static void AssertContractDtoAndViewModelAreIdentical(ContractDto contractDto, ContractViewModel contractViewModel)
    {
      Assert.Equal(contractViewModel.ContractId, contractDto.ContractId);
      Assert.Equal(contractViewModel.EmployeeId, contractDto.EmployeeId);
      Assert.Equal(contractViewModel.Team.TeamId, contractDto.Team.TeamId);
      Assert.Equal(0, contractViewModel.StartDate.CompareTo(contractDto.StartDate));
      Assert.Equal(0, contractViewModel.EndDate.CompareTo(contractDto.EndDate));
    }

    private void GetMockedResourcesGetContractByProjectId(int projectId, IList<ContractDto> serviceReturns,
      out IContractService contractServiceMock, out ContractController contractController, out IMapper mapper)
    {
      contractServiceMock = Substitute.For<IContractService>();
      contractServiceMock.GetContractByProjectId(projectId).Returns(serviceReturns);

      mapper = Substitute.For<IMapper>();
      mapper.Map<IList<ContractViewModel>>(Arg.Any<IList<ContractDto>>()).Returns(new List<ContractViewModel>());

      contractController = new ContractController(mapper, contractServiceMock);
    }
  }
}
