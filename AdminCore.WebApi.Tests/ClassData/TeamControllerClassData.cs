using System.Collections;
using System.Collections.Generic;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Team;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class TeamControllerClassData
    {
        public class GetTeamsByProjectById_ServiceContainsListOfOneTeam_ReturnsOkWithTeamsInBodyClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: projectId: int, serviceReturns: IList<TeamDto>, controllerReturns: IList<TeamViewModel>
                yield return new object[]
                {
                    ProjectId,
                    new List<TeamDto>
                    {
                        new TeamDto {TeamId = 1, TeamName = "unique string 1", ProjectId = 1}
                    },
                    new List<TeamViewModel>
                    {
                        new TeamViewModel {TeamId = 1, TeamName = "unique string 1", ProjectId = 1}
                    }
                };
                yield return new object[]
                {
                    ProjectId,
                    new List<TeamDto>
                    {
                        new TeamDto
                        {
                            TeamId = 1,
                            TeamName = "unique string 1",
                            ProjectId = 1
                        },
                        new TeamDto
                        {
                            TeamId = 1,
                            TeamName = "unique string 99",
                            ProjectId = 1
                        }
                    },
                    new List<TeamViewModel>
                    {
                        new TeamViewModel
                        {
                            TeamId = 1,
                            TeamName = "unique string 1",
                            ProjectId = 1
                        },
                        new TeamViewModel
                        {
                            TeamId = 1,
                            TeamName = "unique string 99",
                            ProjectId = 1
                        }
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
