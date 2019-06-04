using AdminCore.Common.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AdminCore.DTOs.Project;
using AdminCore.WebApi.Models.Client;
using AdminCore.WebApi.Models.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
      try
      {
        var createdProject = _projectService.CreateProject(Mapper.Map<ProjectDto>(projectToCreate));
        return Created(createdProject.ProjectId.ToString(), Mapper.Map<ProjectViewModel>(createdProject));
      }
      catch (DbUpdateException e)
      {
        return BadRequest();
      }
    }

    [HttpPut]
    [ProducesResponseType(typeof(IList<ProjectViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateProject([FromBody] UpdateProjectViewModel projectToUpdate)
    {
      try
      {
        var updatedProject = _projectService.UpdateProject(Mapper.Map<ProjectDto>(projectToUpdate));
        return Ok(Mapper.Map<ProjectViewModel>(updatedProject));
      }
      catch (DbUpdateException e)
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
      catch (DbUpdateException e)
      {
        return BadRequest();
      }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<ProjectViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjects()
    {
      var projectDtoList = _projectService.GetProjects();
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList ?? new List<ProjectDto>()));
    }

    [HttpGet("{projectId}")]
    [ProducesResponseType(typeof(IList<ProjectViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjectsById(int projectId)
    {
      var projectDtoList = _projectService.GetProjectsById(projectId);
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList ?? new List<ProjectDto>()));
    }

    [HttpGet("client/{clientId}")]
    [ProducesResponseType(typeof(IList<ProjectViewModel>), StatusCodes.Status200OK)]
    public IActionResult GetProjectsByClientId(int clientId)
    {
      var projectDtoList = _projectService.GetProjectsByClientId(clientId);
      return Ok(Mapper.Map<IList<ProjectViewModel>>(projectDtoList ?? new List<ProjectDto>()));
    }
  }
}
