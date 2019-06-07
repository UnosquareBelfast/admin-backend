using System.Collections.Generic;
using AdminCore.DTOs.Client;
using AdminCore.DTOs.Team;

namespace AdminCore.DTOs.Project
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }

        public int? ProjectParentId { get; set; }
        public ProjectDto ParentProject { get; set; }

        public string ProjectName { get; set; }

        public int ClientId { get; set; }
        public  ClientDto Client { get; set; }

        public ICollection<TeamDto> Teams { get; set; }
    }
}
