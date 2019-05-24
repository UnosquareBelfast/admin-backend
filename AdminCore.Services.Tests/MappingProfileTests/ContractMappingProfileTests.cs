using System;
using System.Collections;
using System.Collections.Generic;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.Services.Mappings;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace AdminCore.Services.Tests.MappingProfileTests
{
    public class ContractMappingProfileTests
    {
        private IMapper _mapper;

        public ContractMappingProfileTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContractMapperProfile());
            }));
        }

        [Fact]
        public void Map_ContractToContractDto_ValidContractDtoIsProduced()
        {
            // Arrange
            var contractInput = MappingTestObjectBuilder.GetDefaultContract();
            var contractDtoExpected = MappingTestObjectBuilder.GetDefaultContractDto();

            // Act
            var contractDtoActual = _mapper.Map<ContractDto>(contractInput);

            // Assert
            contractDtoExpected.Should().BeEquivalentTo(contractDtoActual);
        }
    }
}
