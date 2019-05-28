using System.Collections.Generic;

namespace AdminCore.WebApi.Models.Project
{
    public class CreateProjectViewModel
    {
        public int? ProjectParentId { get; set; }

        public string ProjectName { get; set; }

        public int ClientId { get; set; }

        public ICollection<int> TeamId { get; set; }
    }
}
