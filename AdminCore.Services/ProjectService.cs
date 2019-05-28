using System.Collections.Generic;
using AdminCore.Common.Interfaces;
using AdminCore.DAL;
using AdminCore.DTOs.Project;
using AdminCore.Services.Base;
using AutoMapper;

namespace AdminCore.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private IMapper _mapper;
        public ProjectService(IMapper mapper, IDatabaseContext databaseContext) : base(databaseContext)
        {
            _mapper = mapper;
        }

        public ProjectDto CreateProject()
        {
            DatabaseContext.Pro
        }

        public ProjectDto UpdateProject()
        {
            throw new System.NotImplementedException();
        }

        public ProjectDto DeleteProject()
        {
            throw new System.NotImplementedException();
        }

        public IList<ProjectDto> GetProjects()
        {
            throw new System.NotImplementedException();
        }

        public IList<ProjectDto> GetProjectsById()
        {
            throw new System.NotImplementedException();
        }

        public IList<ProjectDto> GetProjectsByClientId()
        {
            throw new System.NotImplementedException();
        }
    }
}
