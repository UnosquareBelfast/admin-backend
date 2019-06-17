using System;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Project;
using AdminCore.DTOs.Team;

namespace AdminCore.Services.Tests.MappingProfileTests
{
    public static class MappingTestObjectBuilder
    {
        public static Contract GetDefaultContract()
        {
            return new Contract
            {
                ContractId = 5,
                StartDate = new DateTime(2010, 05, 20),
                EndDate = new DateTime(2011, 08, 13),
                TeamId = 6,
                EmployeeId = 7,
                Team = new Team
                {
                    TeamId = 6,
                    TeamName = TeamName,
                    ProjectId = 15,
//                    ContactName = ContactName,
//                    ContactEmail = ContactEmail,
                    Project = new Project
                    {
                        ProjectId = 15,
                        ProjectName = ProjectName,
                        ClientId = 16,
                        ProjectParentId = 2,
                        Client = new Client
                        {
                            ClientId = 16,
                            ClientName = ClientName
                        }
                    }
                },
                Employee = new Employee
                {
                    EmployeeId = 7
                }
            };
        }

        public static ContractDto GetDefaultContractDto()
        {
            return new ContractDto
            {
                ContractId = 5,
                StartDate = new DateTime(2010, 05, 20),
                EndDate = new DateTime(2011, 08, 13),
                TeamId = 6,
                EmployeeId = 7,
                Team = new TeamDto
                {
                    TeamId = 6,
                    TeamName = TeamName,
                    ProjectId = 15
                },
                ClientName = ClientName,
                ProjectName = ProjectName
            };
        }

        private const string ClientName = "Client Name";
        private const string ProjectName = "Project Name";
        private const string TeamName = "Team Name";
//        private const string ContactName = "Contact Name";
//        private const string ContactEmail = "Contact Email";

        public static Project GetDefaultProject()
        {
            return new Project
            {
                ProjectId = ProjectId,
                ClientId = ClientId,
                ProjectName = ProjectName2,
                ProjectParentId = ProjectParentId
            };
        }

        public static ProjectDto GetDefaultProjectDto()
        {
            return new ProjectDto
            {
                ProjectId = ProjectId,
                ClientId = ClientId,
                ProjectName = ProjectName2,
                ProjectParentId = ProjectParentId
            };
        }

        public static Project GetProjectNullParent()
        {
            return new Project
            {
                ProjectId = ProjectId,
                ClientId = ClientId,
                ProjectName = ProjectName2,
                ProjectParentId = null
            };
        }

        public static ProjectDto GetProjectDtoNullParent()
        {
            return new ProjectDto
            {
                ProjectId = ProjectId,
                ClientId = ClientId,
                ProjectName = ProjectName2,
                ProjectParentId = null
            };
        }

        private const int ProjectId = 1236;
        private const int ClientId = 34;
        private const string ProjectName2 = "Project name";
        private const int ProjectParentId = 56;
    }
}
