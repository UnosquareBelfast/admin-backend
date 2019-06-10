using System.Collections.Generic;
using AdminCore.WebApi.Models.Client;
using AdminCore.WebApi.Models.Team;

namespace AdminCore.WebApi.Models.Project
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        public int? ProjectParentId { get; set; }
        public ProjectViewModel ParentProject { get; set; }

        public string ProjectName { get; set; }

        public int ClientId { get; set; }
        public ClientViewModel Client { get; set; }

        public ICollection<TeamViewModel> Teams { get; set; }
    }
}
