using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DAL.Models;
using AdminCore.DTOs.Dashboard;
using AdminCore.DTOs.Employee;
using AutoFixture;

namespace AdminCore.Services.Tests.ClassData
{
    public class DashboardServiceClassData
    {
        public class DashboardEventsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                var dateTimeToGet = new DateTime(2019, 07, 06 );

                var employees = new EmployeeSnapshotDto
                {
                    EmployeeId = EmployeeId
                };

                var teams = new TeamSnapshotDto
                {
                    TeamId = 1,
                    Employees = new List<EmployeeSnapshotDto>{ employees }
                };

                var projects = new ProjectSnapshotDto
                {
                    ProjectId = 1,
                    Teams = new List<TeamSnapshotDto> { teams }
                };

                var clients = new ClientSnapshotDto
                {
                    ClientId = 1,
                    Projects = new List<ProjectSnapshotDto> { projects }
                };

                // ARGS: employeeId: int, DateTime dateToGet, teamRepoOut IList<Team>, clientRepoOut IQueryable<Client>,
                // clientSnapshotMapOut: ClientSnapshotDto, projectSnapshotMapOut: ProjectSnapshotDto, teamSnapshotMapOut: TeamSnapshotDto, employeeSnapshotMapOut: EmployeeSnapshotDto
                yield return new object[]
                {
                    EmployeeId,
                    dateTimeToGet,
                    new List<Team>
                    {
                        new Team
                        {
                            TeamId = 1,
                            ProjectId = 1
                        }
                    },
                    new List<Client>
                    {
                        new Client
                        {
                            ClientId = 1,
                            Projects = new List<Project>
                            {
                                new Project
                                {
                                    ProjectId = 1,
                                    Teams = new List<Team>
                                    {
                                        new Team
                                        {
                                            TeamId = 1,
                                            Contracts = new List<Contract>
                                            {
                                                new Contract
                                                {
                                                    ContractId = 1,
                                                    TeamId = 1,
                                                    EmployeeId = 1,
                                                    Employee = new Employee
                                                    {
                                                        EmployeeId = 1,
                                                        Events = new List<Event>
                                                        {
                                                            new Event
                                                            {
                                                                EventId = 1,
                                                                EventDates = new List<EventDate>
                                                                {
                                                                    new EventDate
                                                                    {
                                                                        EventDateId = 1,
                                                                        StartDate = dateTimeToGet,
                                                                        EndDate = dateTimeToGet
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }.AsQueryable(),
                    clients,
                    projects,
                    teams,
                    employees
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int EmployeeId = 6;
    }
}
