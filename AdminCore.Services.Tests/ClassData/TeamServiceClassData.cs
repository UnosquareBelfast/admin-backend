using System.Collections;
using System.Collections.Generic;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class TeamServiceClassData
    {
        public class adadsdasdasdas : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                // ARGS: employeeId: int, DateTime dateToGet, teamRepoOut IList<Team>, clientRepoOut IQueryable<Client>,
                // clientSnapshotMapOut: ClientSnapshotDto, projectSnapshotMapOut: ProjectSnapshotDto, teamSnapshotMapOut: TeamSnapshotDto, employeeSnapshotMapOut: EmployeeSnapshotDto
                yield return new object[]
                {
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
