using AdminCore.Common.Interfaces;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Team;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AdminCore.DTOs.Project;
using AdminCore.Services.Mappings;
using AdminCore.WebApi.Models.Client;
using AdminCore.WebApi.Models.Project;
using Microsoft.AspNetCore.Http;

namespace AdminCore.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProjectController : BaseController
  {
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService, IMapper mapper) : base(mapper)
    {
      _projectService = projectService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateProjectViewModel), StatusCodes.Status201Created)]
    public IActionResult CreateProject([FromBody] CreateProjectViewModel projectToCreate)
    {
      var createdObj = _projectService.CreateProject(Mapper.Map<ProjectDto>(projectToCreate));
      return Created(createdObj.ProjectId.ToString(), Mapper.Map<ProjectViewModel>(createdObj));
    }

    [HttpPut]
    [ProducesResponseType(typeof(IList<ClientViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateProject([FromBody] UpdateProjectViewModel projectToUpdate)
    {
      try
      {
        var updatedObj = _projectService.UpdateProject(Mapper.Map<ProjectDto>(projectToUpdate));
        return Ok(Mapper.Map<ProjectViewModel>(updatedObj));
      }
      catch (Exception e)
      {
        return BadRequest();
      }
    }

    [HttpDelete("{projectId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult DeleteProject(int projectId)
    {
      try
      {
        _projectService.DeleteProject(projectId);
        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest();
      }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ClientViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjects()
    {
      var projectDtoList = _projectService.GetProjects();
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList));
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType(typeof(IList<ClientViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjectById(int projectId)
    {
      var projectDtoList = _projectService.GetProjectsById(projectId);
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList));
    }

    [HttpGet("client/{clientId}")]
    [ProducesResponseType(typeof(IList<ClientViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjectsByClientId(int clientId)
    {
      var projectDtoList = _projectService.GetProjectsByClientId(clientId);
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList));
    }
  }
}
