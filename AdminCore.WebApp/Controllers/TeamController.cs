﻿using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Team;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace AdminCore.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TeamController : BaseController
  {
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService, IMapper mapper) : base(mapper)
    {
      _teamService = teamService;
    }

    [HttpGet]
    public IActionResult GetAllTeams()
    {
      var teams = _teamService.GetAll();
      if (teams.Any())
      {
        return Ok(Mapper.Map<List<TeamViewModel>>(teams));
      }

      return StatusCode((int)HttpStatusCode.InternalServerError, "No teams exist");
    }

    [HttpGet("{id}")]
    public IActionResult GetTeamById(int id)
    {
      var team = _teamService.Get(id);
      if (team != null)
      {
        return Ok(Mapper.Map<TeamViewModel>(team));
      }

      return StatusCode((int)HttpStatusCode.InternalServerError, $"No team found with an ID of { id.ToString() }");
    }

    [HttpGet("getTeamsByProjectId/{projectId}")]
    [ProducesResponseType(typeof(IList<TeamViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetTeamsByProjectById(int projectId)
    {
      var teamList = _teamService.GetByProjectId(projectId);
      if (teamList == null || !teamList.Any())
      {
        return NoContent();
      }

      return Ok(Mapper.Map<IList<TeamViewModel>>(teamList));
    }

    [HttpPost]
    public IActionResult CreateTeam(CreateTeamViewModel teamViewModel)
    {
      var teamDto = Mapper.Map<TeamDto>(teamViewModel);
      TeamDto savedTeam;
      try
      {
        savedTeam = _teamService.Save(teamDto);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong, team was not created.");
      }

      return Ok(savedTeam);
    }

    [HttpPut]
    public IActionResult UpdateTeam(UpdateTeamViewModel teamViewModel)
    {
      var teamDto = Mapper.Map<TeamDto>(teamViewModel);
      try
      {
        _teamService.Save(teamDto);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex.Message);
        return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong, team was not updated.");
      }
      return Ok();
    }

    [HttpGet("getByClientId/{clientId}")]
    public IActionResult GetAllTeamsForClientId(int clientId)
    {
      var teams = _teamService.GetByClientId(clientId);
      if (teams.Any())
      {
        return Ok(Mapper.Map<IList<TeamViewModel>>(teams));
      }

      return StatusCode((int)HttpStatusCode.NoContent, $"No teams found with client ID {clientId}");
    }
  }
}
