using System;
using System.Collections.Generic;
using AdminCore.Common.Authorization;
using AdminCore.Extensions.Tests.ClassData;
using Xunit;
using FluentAssertions;

namespace AdminCore.Extensions.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [ClassData(typeof(StringExtensionsClassData.ValidStringWithSeparatorsClassData))]
        public void SplitStringBySeparatorAndTakeRange_ValidStringWithSeparators_ValidSplitListReturned(string stringToSplit, char separator, int rangeMin, IList<string> splitExpected)
        {
            // Arrange
            // Act
            var stringActual = stringToSplit.SplitStringBySeparatorAndTakeRange(separator, rangeMin);

            // Assert
            splitExpected.Should().BeEquivalentTo(stringActual);
        }

        [Theory]
        [ClassData(typeof(StringExtensionsClassData.ValidStringWithIncorrectSeparatorsClassData))]
        public void SplitStringBySeparatorAndTakeRange_ValidStringWithIncorrectSeparators_ListWithStringToSplitReturned(string stringToSplit, char separator, int rangeMin, IList<string> splitExpected)
        {
            // Arrange
            // Act
            var stringActual = stringToSplit.SplitStringBySeparatorAndTakeRange(separator, rangeMin);

            // Assert
            splitExpected.Should().BeEquivalentTo(stringActual);
        }

        [Theory]
        [ClassData(typeof(StringExtensionsClassData.ValidStringWithSeparatorsAndInvalidRangeMinClassData))]
        public void SplitStringBySeparatorAndTakeRange_ValidStringWithSeparatorsAndInvalidRangeMin_ExceptionEncountered(string stringToSplit, char separator, int rangeMin)
        {
            // Arrange
            Func<IList<string>> action = () => stringToSplit.SplitStringBySeparatorAndTakeRange(separator, rangeMin);
            // Act
            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void test()
        {
            AdminCoreRolesHandler a = new AdminCoreRolesHandler();
            a.
        }
    }
}
