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
using AdminCore.WebApi.Models.Project;

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

    public IActionResult CreateProject(CreateProjectViewModel projectToSave)
    {
      throw new NotImplementedException();
    }

    public IActionResult UpdateProject(UpdateTeamViewModel projectToUpdate)
    {
      throw new NotImplementedException();
    }

    public IActionResult DeleteProject(int projectId)
    {
      throw new NotImplementedException();
    }

    public IActionResult GetProjects()
    {
      throw new NotImplementedException();
    }

    public IActionResult GetProjectsById(int projectId)
    {
      throw new NotImplementedException();
    }

    public IActionResult GetProjectsByClientId(int clientId)
    {
      throw new NotImplementedException();
    }
  }
}
