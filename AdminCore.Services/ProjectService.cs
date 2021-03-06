using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Project;
using AdminCore.Services.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AdminCore.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IMapper _mapper;
        public ProjectService(IMapper mapper, IDatabaseContext databaseContext) : base(databaseContext)
        {
            _mapper = mapper;
        }

        public ProjectDto CreateProject(ProjectDto projectToCreate)
        {
            var project = _mapper.Map<Project>(projectToCreate);
            var savedProject = DatabaseContext.ProjectRepository.Insert(project);
            DatabaseContext.SaveChanges();

            return _mapper.Map<ProjectDto>(savedProject);
        }

        public ProjectDto UpdateProject(ProjectDto projectToUpdate)
        {
            var project = _mapper.Map<Project>(projectToUpdate);
            var updateProject = DatabaseContext.ProjectRepository.Update(project);
            DatabaseContext.SaveChanges();

            return _mapper.Map<ProjectDto>(updateProject);
        }

        public void DeleteProject(int projectId)
        {
            DatabaseContext.ProjectRepository.Delete(projectId);
            DatabaseContext.SaveChanges();
        }

        public IList<ProjectDto> GetProjects(int? projectId = null, int? clientId = null)
        {
            Expression<Func<Project, bool>> filterExpression = project =>
                (projectId == null || project.ProjectId == projectId) &&
                (clientId == null || project.ClientId == clientId);

            var projectList = DatabaseContext.ProjectRepository.Get(filterExpression, null,
                project => project.Client,
                project => project.Teams,
                project => project.ParentProject);
            return _mapper.Map<IList<ProjectDto>>(projectList) ?? new List<ProjectDto>();
        }
    }
}
