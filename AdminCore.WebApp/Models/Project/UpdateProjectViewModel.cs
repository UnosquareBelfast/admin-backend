using System.Collections.Generic;
using AdminCore.WebApi.Models.Client;
using AdminCore.WebApi.Models.Team;

namespace AdminCore.WebApi.Models.Project
{
    public class UpdateProjectViewModel
    {
        public int ProjectId { get; set; }

        public int? ProjectParentId { get; set; }

        public string ProjectName { get; set; }

        public int ClientId { get; set; }

        public ICollection<int> TeamIds { get; set; }
    }
}
