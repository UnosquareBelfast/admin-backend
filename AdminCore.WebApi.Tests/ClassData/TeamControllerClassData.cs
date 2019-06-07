using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Team;
using AutoFixture;

namespace AdminCore.WebApi.Tests.ClassData
{
    public class TeamControllerClassData
    {
        public class GetTeamsByProjectById_ServiceContainsListOfOneTeam_ReturnsOkWithTeamsInBodyClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: projectId: int, serviceReturns: IList<TeamDto>, controllerReturns: IList<TeamViewModel>
                yield return new object[]
                {
                    ProjectId,
                    fixture.CreateMany<TeamDto>().ToList(),
                    fixture.CreateMany<TeamViewModel>().ToList(),
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
