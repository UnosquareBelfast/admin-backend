using AdminCore.Common.Interfaces;
using AdminCore.DTOs;
using AdminCore.WebApi.Models.Contract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdminCore.WebApi.Validators;
using Microsoft.AspNetCore.Http;

namespace AdminCore.WebApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class ContractController : BaseController
  {
    private readonly IContractService _contractService;

    public ContractController(IMapper mapper, IContractService contractService) : base(mapper)
    {
      _contractService = contractService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IList<ContractViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetContractById(int id)
    {
      var contractDto = _contractService.GetContractById(id);
      if (contractDto != null)
      {
        return Ok(Mapper.Map<ContractViewModel>(contractDto));
      }

      return NoContent();
    }

    [HttpGet("getByEmployeeId/{employeeId}")]
    [ProducesResponseType(typeof(IList<ContractViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetContractByEmployeeId(int employeeId)
    {
      var contractDtos = _contractService.GetContractByEmployeeId(employeeId);
      if (contractDtos.Any())
      {
        return Ok(Mapper.Map<IList<ContractViewModel>>(contractDtos));
      }

      return NoContent();
    }

    [HttpGet("getByTeamId/{teamId}")]
    [ProducesResponseType(typeof(IList<ContractViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetContractByTeamId(int teamId)
    {
      var contractDtos = _contractService.GetContractByTeamId(teamId);
      if (contractDtos.Any())
      {
        return Ok(Mapper.Map<IList<ContractViewModel>>(contractDtos));
      }

      return NoContent();
    }

    [HttpGet("getByProjectId/{projectId}")]
    [ProducesResponseType(typeof(IList<ContractViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetContractByProjectId(int projectId)
    {
      var contractDtos = _contractService.GetContractByProjectId(projectId);
      if (contractDtos == null || !contractDtos.Any())
      {
        return NoContent();
      }

      return Ok(Mapper.Map<IList<ContractViewModel>>(contractDtos));
    }

    [HttpGet("getByEmployeeIdAndTeamId/{employeeId}/{teamId}")]
    [ProducesResponseType(typeof(IList<ContractViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetContractByEmployeeIdAndTeamId(int employeeId, int teamId)
    {
      var contractDtos = _contractService.GetContractByEmployeeIdAndTeamId(employeeId, teamId);
      if (contractDtos.Any())
      {
        return Ok(Mapper.Map<IList<ContractViewModel>>(contractDtos));
      }

      return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateContract(CreateContractViewModel contract)
    {
      var contractDto = Mapper.Map<ContractDto>(contract);
      try
      {
        ValidateCreateContract(contract);
        _contractService.SaveContract(contractDto);
        return Ok("Contract successfully created.");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode(500, $"Something went wrong. Contract was not created: {ex.Message}");
      }
    }

    private void ValidateCreateContract(CreateContractViewModel contract)
    {
      var createContractValidator = new CreateContractValidator(_contractService, Mapper);
      createContractValidator.Validate(contract);
    }

    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateContract(UpdateContractViewModel contract)
    {
      var contractDto = Mapper.Map<ContractDto>(contract);
      try
      {
        _contractService.SaveContract(contractDto);
        return Ok("Contract successfully updated.");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode(500, "Something went wrong. Contract was not updated.");
      }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteContract(int id)
    {
      try
      {
        _contractService.DeleteContract(id);
        return Ok("Contract successfully deleted.");
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode(500, "Something went wrong. Contract was not deleted.");
      }
    }
  }
}
