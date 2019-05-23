using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.Services.Base;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdminCore.Services
{
  public class ContractService : BaseService, IContractService
  {
    private readonly IMapper _mapper;

    public ContractService(IDatabaseContext databaseContext, IMapper mapper) : base(databaseContext)
    {
      _mapper = mapper;
    }

    public ContractDto GetContractById(int contractId)
    {
      return _mapper.Map<ContractDto>(GetById(contractId));
    }

    public IList<ContractDto> GetContractByEmployeeId(int employeeId)
    {
      var contractList = DatabaseContext.ContractRepository.Get(x => x.EmployeeId == employeeId, null,
                                              x => x.Team, x => x.Team.Project.Client);
      return _mapper.Map<IList<ContractDto>>(contractList);
    }

    public IList<ContractDto> GetContractByTeamId(int teamId)
    {
      var contractList = DatabaseContext.ContractRepository.Get(x => x.TeamId == teamId, null,
                                              x => x.Team, x => x.Team.Project.Client);
      return _mapper.Map<IList<ContractDto>>(contractList);
    }

    public IList<ContractDto> GetContractByEmployeeIdAndTeamId(int employeeId, int teamId)
    {
      var contractList = DatabaseContext.ContractRepository.Get(x => x.TeamId == teamId && x.EmployeeId == employeeId, null,
                                              x => x.Team, x => x.Team.Project.Client);
      return _mapper.Map<IList<ContractDto>>(contractList);
    }

    public void SaveContract(ContractDto contractToBeSaved)
    {
      if (contractToBeSaved.ContractId == 0)
      {
        InsertNewContractIntoTheDb(contractToBeSaved);
      }
      else
      {
        UpdateExistingContract(contractToBeSaved);
      }
      DatabaseContext.SaveChanges();
    }

    public void DeleteContract(int contractId)
    {
      var contractToDelete = GetById(contractId);
      if (contractToDelete == null)
      {
        throw new Exception($"Tried to delete contract with ID {contractId} but a contract with this ID could not be found.");
      }
      DatabaseContext.ContractRepository.Delete(contractToDelete);
      DatabaseContext.SaveChanges();
    }

    private Contract GetById(int id)
    {
      return DatabaseContext.ContractRepository.GetSingle(x => x.ContractId == id,
                                                   x => x.Team,
                                                                x => x.Team.Project.Client);
    }

    public bool ContractAlreadyExists(ContractDto newContract)
    {
      var identicalContracts = DatabaseContext.ContractRepository.Get
      (
        existingContract =>
          existingContract.EmployeeId == newContract.EmployeeId &&
          existingContract.TeamId == newContract.TeamId &&
          DateService.ContractDatesOverlap(_mapper.Map<Contract>(newContract), existingContract)
      );
      return identicalContracts.Any();
    }

    private void UpdateExistingContract(ContractDto contractToBeSaved)
    {
      var existingEntry = GetById(contractToBeSaved.ContractId);
      if (existingEntry == null)
      {
        throw new Exception($"Tried to update contract with ID {contractToBeSaved.ContractId} but a contract with this ID could not be found.");
      }
      _mapper.Map(contractToBeSaved, existingEntry);
    }

    private void InsertNewContractIntoTheDb(ContractDto contractToBeSaved)
    {
      var newDbEntry = _mapper.Map<Contract>(contractToBeSaved);
      DatabaseContext.ContractRepository.Insert(newDbEntry);
    }
  }
}
