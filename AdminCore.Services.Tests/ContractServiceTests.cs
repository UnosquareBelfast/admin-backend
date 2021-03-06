﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Database;
using AdminCore.DAL.Entity_Framework;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Team;
using AdminCore.Services.Mappings;
using AdminCore.Services.Tests.ClassData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace AdminCore.Services.Tests
{
  public class ContractServiceTests : BaseMockedDatabaseSetUp
  {
    private readonly IContractService _contractService;
    private readonly IDatabaseContext _databaseContext;

    private static readonly IMapper Mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.AddProfile(new ContractMapperProfile());
      cfg.AddProfile(new ClientMapperProfile());
    }));
    private static readonly IConfiguration Configuration = Substitute.For<IConfiguration>();
    private static readonly AdminCoreContext AdminCoreContext = Substitute.For<AdminCoreContext>(Configuration);

    public ContractServiceTests()
    {
      _databaseContext = Substitute.For<IDatabaseContext>();
      IEnumerable<Type> profiles = new List<Type>
      {
        new ContractMapperProfile().GetType(),
        new TeamMapperProfile().GetType()
      };
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(profiles)));
      _contractService = new ContractService(_databaseContext, mapper);
    }

    [Fact]
    public void TestGetContractByIdReturnsNotNullContractDtoWhenDatabaseReturnsContract()
    {
      var contract = BuildContractModel();
      _databaseContext.ContractRepository.GetSingle(Arg.Any<Expression<Func<Contract, bool>>>(), Arg.Any<Expression<Func<Contract, object>>>(), Arg.Any<Expression<Func<Contract, object>>>()).Returns(contract);

      var result = _contractService.GetContractById(contract.ContractId);
      AssertContractAndContractDtoAreIdentical(contract, result);
    }

    [Fact]
    public void TestGetContractByIdReturnsNullContractDtoWhenDatabaseReturnsNull()
    {
      var result = _contractService.GetContractById(1);
      Assert.Null(result);
    }

    [Fact]
    public void TestGetContractByEmployeeIdReturnsNotNullContractDtoListWhenDatabaseReturnsContractList()
    {
      // Arrange
      var contract = BuildContractModel();
      var contractList = new List<Contract>{ contract };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpGenericRepository(databaseContext, contractList,
        repository => { databaseContext.Configure().ContractRepository.Returns(repository); }, AdminCoreContext);

      var contractService = new ContractService(databaseContext, Mapper);

      // Act
      var result = contractService.GetContractByEmployeeId(1);

      // Assert
      AssertContractAndContractDtoAreIdentical(contract, result.First());
    }

    [Fact]
    public void TestGetContractByEmployeeIdReturnsEmptyContractDtoListWhenDatabaseReturnsNothing()
    {
      var result = _contractService.GetContractByEmployeeId(1);
      Assert.Empty(result);
    }

    [Fact]
    public void TestGetContractByTeamIdReturnsNotNullContractDtoListWhenDatabaseReturnsContractList()
    {
      // Arrange
      var contract = BuildContractModel();
      var contractList = new List<Contract>{ contract };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpGenericRepository(databaseContext, contractList,
        repository => { databaseContext.Configure().ContractRepository.Returns(repository); }, AdminCoreContext);

      var contractService = new ContractService(databaseContext, Mapper);

      // Act
      var result = contractService.GetContractByTeamId(1);

      // Assert
      AssertContractAndContractDtoAreIdentical(contract, result.First());
    }

    [Fact]
    public void TestGetContractByTeamIdReturnsEmptyContractDtoListWhenDatabaseReturnsNothing()
    {
      var result = _contractService.GetContractByTeamId(1);
      Assert.Empty(result);
    }

    [Theory]
    [ClassData(typeof(ContractServiceClassData.ProjectIdFixtureListDataGetContractByProjectId))]
    public void GetContractByProjectId_ContractRepoReturnsNonEmptyList_ServiceReturnsCorrectNumberOfItemsAndCorrectCallsReceived(int projectId, IList<Contract> dbReturns,
      IList<ContractDto> serviceExpected)
    {
      // Arrange
      var contractServiceMock = GetMockedResourcesGetContractByProjectId(dbReturns, out var ormContext, out var mapper);

      // Act
      var serviceActual = contractServiceMock.GetContractByProjectId(projectId);

      // Assert
      ormContext.ContractRepository.Received(1).Get();
      serviceActual.Should().BeEquivalentTo(serviceExpected);
    }

    [Fact]
    public void GetContractByProjectId_ContractRepoReturnsEmptyList_ServiceReturnsEmptyListAndCorrectCallsReceived()
    {
      // Arrange
      var contractServiceMock = GetMockedResourcesGetContractByProjectId(new List<Contract>(), out var ormContext, out var mapper);

      // Act
      var serviceActual = contractServiceMock.GetContractByProjectId(234);

      // Assert
      ormContext.ContractRepository.Received(1).Get();
      serviceActual.Should().BeEquivalentTo(new List<ContractDto>());
    }

    [Fact]
    public void GetContractByEmployeeIdAndTeamId_ContractRepoHasOneContract_ReturnsContractDtoThatMatchesContract()
    {
      // Arrange
      var contract = BuildContractModel();
      var contractList = new List<Contract>{ contract };

      var databaseContext = Substitute.ForPartsOf<EntityFrameworkContext>(AdminCoreContext);
      databaseContext = SetUpGenericRepository(databaseContext, contractList,
        repository => { databaseContext.Configure().ContractRepository.Returns(repository); }, AdminCoreContext);

      var contractService = new ContractService(databaseContext, Mapper);

      // Act
      var result = contractService.GetContractByEmployeeIdAndTeamId(1, 1);

      // Assert
      AssertContractAndContractDtoAreIdentical(contract, result.First());
    }

    [Fact]
    public void TestGetContractByEmployeeIdAndTeamIdReturnsEmptyContractDtoListWhenDatabaseReturnsNothing()
    {
      var result = _contractService.GetContractByEmployeeIdAndTeamId(2, 2);
      Assert.Empty(result);
    }

    [Fact]
    public void TestDeleteContractAttemptsDeleteIfExistingContractIsFound()
    {
      var contract = BuildContractModel();

      var databaseContext = Substitute.For<IDatabaseContext>().Configure();
      IEnumerable<Type> profiles = new List<Type>
      {
        new ContractMapperProfile().GetType(),
        new TeamMapperProfile().GetType()
      };
      IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfiles(profiles)));


      var contractService = new ContractService(databaseContext, mapper);

      databaseContext.ContractRepository.GetSingle(Arg.Any<Expression<Func<Contract, bool>>>(),
        Arg.Any<Expression<Func<Contract, object>>>(), Arg.Any<Expression<Func<Contract, object>>>()).Returns(contract);

      contractService.DeleteContract(contract.ContractId);
      databaseContext.ContractRepository.Received(1).Delete(contract);

    }

    [Fact]
    public void TestDeleteContractThrowsExceptionIfExistingContractIsNotFound()
    {
      _databaseContext.ContractRepository.GetSingle(Arg.Any<Expression<Func<Contract, bool>>>()).ReturnsNull();
      Assert.Throws<Exception>(() => _contractService.DeleteContract(1));
    }

    [Fact]
    public void TestSaveContractAttemptsInsertIfContractToBeSavedHasIdOfZero()
    {
      var newContractDto = BuildContractDto();
      newContractDto.ContractId = 0;
      _contractService.SaveContract(newContractDto);
      _databaseContext.ContractRepository.Received(1).Insert(Arg.Any<Contract>());
    }

    [Fact]
    public void TestSaveContractAttemptsUpdateIfContractToBeSavedHasIdOfNotZeroAndContractExists()
    {
      var newContractDto = BuildContractDto();
      _databaseContext.ContractRepository.GetSingle(Arg.Any<Expression<Func<Contract, bool>>>(), Arg.Any<Expression<Func<Contract, object>>>(), Arg.Any<Expression<Func<Contract, object>>>()).Returns(BuildContractModel());
      _contractService.SaveContract(newContractDto);
      _databaseContext.ContractRepository.Received(0).Insert(Arg.Any<Contract>());
      _databaseContext.Received(1).SaveChanges();
    }

    [Fact]
    public void TestSaveContractThrowsExceptionIfContractToBeSavedHasIdOfNotZeroAndContractDoesNotExist()
    {
      var newContractDto = BuildContractDto();
      Assert.Throws<Exception>(() => _contractService.SaveContract(newContractDto));
    }

    private static Contract BuildContractModel()
    {
      return new Contract
      {
        ContractId = 1,
        TeamId = 1,
        EmployeeId = 1,
        StartDate = new DateTime(2018, 12, 10),
        EndDate = new DateTime(2019, 1, 10),
        Team = new Team
        {
          TeamId = 1,
          ProjectId = 1,
          Project = new Project
          {
            ProjectId = 1,
            ClientId = 1,
            ProjectName = "Project Name",
            Client = new Client
            {
              ClientId = 1,
              ClientName = "Client Name"
            }
          }
        }
      };
    }

    private static ContractDto BuildContractDto()
    {
      return new ContractDto
      {
        ContractId = 1,
        EmployeeId = 1,
        Team = new TeamDto
        {
          TeamId = 1
        },
        StartDate = new DateTime(2018, 12, 10),
        EndDate = new DateTime(2019, 1, 10)
      };
    }

    private static void AssertContractAndContractDtoAreIdentical(Contract contract, ContractDto contractDto)
    {
      Assert.Equal(contract.Team.Project.ProjectName, contractDto.ProjectName);
      Assert.Equal(contract.Team.Project.Client.ClientName, contractDto.ClientName);
      Assert.Equal(contract.ContractId, contractDto.ContractId);
      Assert.Equal(contract.EmployeeId, contractDto.EmployeeId);
      Assert.Equal(contract.TeamId, contractDto.Team.TeamId);
      Assert.Equal(0, contract.StartDate.CompareTo(contractDto.StartDate));
      if (contract.EndDate.HasValue)
      {
        Assert.Equal(0, contract.EndDate.Value.CompareTo(contractDto.EndDate));
      }
    }

    private ContractService GetMockedResourcesGetContractByProjectId(IList<Contract> dbReturns, out EntityFrameworkContext ormContext, out IMapper mapper)
    {
      var efContext = SetupMockedOrmContext(out var dbContext);
      ormContext = SetUpGenericRepository(efContext, dbReturns,
        repository => { efContext.Configure().ContractRepository.Returns(repository); }, dbContext);

      mapper = Substitute.ForPartsOf<Mapper>(new MapperConfiguration(cfg => cfg.AddProfile(new ContractMapperProfile()))).Configure();

      return new ContractService(ormContext, mapper);
    }
  }
}
