using System;
using AdminCore.Common.Authorization;
using AdminCore.Constants.Enums;
using FluentAssertions;
using Xunit;

namespace AdminCore.Common.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("Admin", EmployeeRoles.SystemAdministrator)]
        [InlineData("Standard", EmployeeRoles.User)]
        [InlineData("TeamLeader", EmployeeRoles.TeamLeader)]
        public void ConvertToEmployeeRoles_ValidAzureRolesProvided_CorrectEmployeeRoleReturned(string azureRole, EmployeeRoles employeeRoleExpected)
        {
            // Arrange
            // Act
            var employeeRoleActual = AzureRoleEnumConvert.ConvertToEmployeeRoles(azureRole);

            // Assert
            employeeRoleExpected.Should().Be(employeeRoleActual);
        }

        [Theory]
        [InlineData("Standard1")]
        [InlineData("Standard  ")]
        [InlineData("")]
        public void ConvertToEmployeeRoles_InvalidAzureRolesProvided_InvalidOperationExceptionEncountered(string azureRole)
        {
            // Arrange
            Func<EmployeeRoles> action = () => AzureRoleEnumConvert.ConvertToEmployeeRoles(azureRole);

            // Act
            // Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
