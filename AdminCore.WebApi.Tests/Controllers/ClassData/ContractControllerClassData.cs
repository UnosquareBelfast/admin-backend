using System.Collections;
using System.Collections.Generic;
using AdminCore.DTOs;
using AdminCore.DTOs.Team;
using AdminCore.WebApi.Models.Contract;
using AdminCore.WebApi.Models.Team;

namespace AdminCore.WebApi.Tests.Controllers.ClassData
{
    public class ContractControllerClassData
    {
        public class GetContractByProjectId_ServiceContainsListOfTwoTeams_ReturnsOkWithTeamsInBody : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: inputDataList: List<DataETLTestModelSimple>, expectedCsv: string
                yield return new object[]
                {
                    ProjectId,
                    new List<ContractDto>
                    {
                        new ContractDto {ContractId = 1, Team = new TeamDto {ProjectId = ProjectId}}
                    },
                    new List<ContractViewModel>
                    {
                        new ContractViewModel {ContractId = 1, Team = new TeamViewModel {ProjectId = ProjectId}}
                    }
                };
                yield return new object[]
                {
                    ProjectId,
                    new List<ContractDto>
                    {
                        new ContractDto {ContractId = 1, Team = new TeamDto {ProjectId = ProjectId}},
                        new ContractDto {ContractId = 2, Team = new TeamDto {ProjectId = ProjectId}}
                    },
                    new List<ContractViewModel>
                    {
                        new ContractViewModel {ContractId = 1, Team = new TeamViewModel {ProjectId = ProjectId}},
                        new ContractViewModel {ContractId = 2, Team = new TeamViewModel {ProjectId = ProjectId}}
                    }
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private const int ProjectId = 5;
    }
}
