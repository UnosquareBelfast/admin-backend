using System;
using System.Collections;
using System.Collections.Generic;
using AdminCore.DAL.Models;
using AdminCore.DTOs;
using AdminCore.DTOs.Project;
using AdminCore.Services.Mappings;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace AdminCore.Services.Tests.MappingProfileTests
{
    public class ProjectMappingProfileTests
    {
        private IMapper _mapper;

        public ProjectMappingProfileTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProjectMapperProfile());
            }));
        }

        [Fact]
        public void Map_ProjectToProjectDto_ValidContractDtoIsProduced()
        {
            // Arrange
            var project = MappingTestObjectBuilder.GetDefaultProject();
            var projectDto = MappingTestObjectBuilder.GetDefaultProjectDto();

            // Act
            var projectDtoActual = _mapper.Map<ProjectDto>(project);
            var projectActual = _mapper.Map<Project>(projectDto);

            // Assert
            projectDto.Should().BeEquivalentTo(projectDtoActual);
            project.Should().BeEquivalentTo(projectActual);
        }
    }
}
