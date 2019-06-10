using System.Collections;
using System.Collections.Generic;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Team;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class TeamServiceClassData
    {
        public class TeamTeamDtosWithProjectIdClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                int projectId = fixture.Create<int>();

                // ARGS: employeeId: int, DateTime dateToGet, teamRepoOut IList<Team>, clientRepoOut IQueryable<Client>,
                // clientSnapshotMapOut: ClientSnapshotDto, projectSnapshotMapOut: ProjectSnapshotDto, teamSnapshotMapOut: TeamSnapshotDto, employeeSnapshotMapOut: EmployeeSnapshotDto
                yield return new object[]
                {
                    projectId,
                    new List<Team>
                    {
                        new Team{ProjectId = projectId}
                    },
                    new List<TeamDto>
                    {
                        new TeamDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Team>
                    {
                        new Team{ProjectId = projectId},
                        new Team{ProjectId = projectId}
                    },
                    new List<TeamDto>
                    {
                        new TeamDto{ProjectId = projectId},
                        new TeamDto{ProjectId = projectId}
                    }
                };
                yield return new object[]
                {
                    projectId,
                    new List<Team>
                    {
                        new Team{ProjectId = projectId},
                        new Team{ProjectId = projectId + 1}
                    },
                    new List<TeamDto>
                    {
                        new TeamDto{ProjectId = projectId}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
