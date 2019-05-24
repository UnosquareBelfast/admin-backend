using System;
using System.Collections.Generic;
using System.Linq;
using AdminCore.DataETL.Tests.ClassData;
using AdminCore.DataETL.Tests.Models;
using ChoETL;
using NSubstitute;
using Xunit;

namespace AdminCore.DataETL.Tests
{
    public class CsvFileTransformAdapterTests
    {
        [Fact]
        public void GenerateByteArray_NullUsedAsCsvDataSource_ReturnedByArrayIsEmptyNotNull()
        {
            //Arrange
            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            // Act
            var csvResult = csvFileTransformAdapter.GenerateByteArray<DataETLTestModelSimple>(null);

            // Assert
            Assert.True(!csvResult.Any());
        }

        [Fact]
        public void GenerateByteArray_NegativeNumberUsedOnExportableRecordFieldColumn_ThrowsException()
        {
            //Arrange
            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            var data = new List<DataETLTestModelRecordFieldNegativeNumber>
            {
                new DataETLTestModelRecordFieldNegativeNumber
                {
                    Field_Int1 = 2
                }
            };

            // Act
            // Assert
            Assert.Throws<ChoRecordConfigurationException>(() => csvFileTransformAdapter.GenerateByteArray(data));
        }

        [Theory]
        [ClassData(typeof(CsvFileTransformAdapterClassData.GenerateByteArray_DataETLTestModelSimpleWithValidData_ReturnedByteArrayMatchesExpected_ClassData))]
        public void GenerateByteArray_DataETLTestModelSimpleWithValidData_ReturnedByteArrayMatchesExpected(
            IList<DataETLTestModelSimple> inputDataList, string expectedCsv)
        {
            //Arrange
            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            // Act
            var csvResult = csvFileTransformAdapter.GenerateByteArray(inputDataList);
            var actualCsv = System.Text.Encoding.Default.GetString(csvResult);

            // Assert
            Assert.Equal(expectedCsv, actualCsv);
        }

        [Theory]
        [ClassData(typeof(CsvFileTransformAdapterClassData.GenerateByteArray_DataETLTestModelIgnoreWithValidData_ReturnedByteArrayMatchesExpected))]
        public void GenerateByteArray_DataETLTestModelIgnoreWithValidData_ReturnedByteArrayMatchesExpected(
            IList<DataETLTestModelIgnore> inputDataList, string expectedCsv)
        {
            //Arrange
            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            // Act
            var csvResult = csvFileTransformAdapter.GenerateByteArray(inputDataList);
            var actualCsv = System.Text.Encoding.Default.GetString(csvResult);

            // Assert
            Assert.Equal(expectedCsv, actualCsv);
        }

        [Fact]
        public void GenerateByteArray_DataETLTestModelDuplicateRecordField_ThrowsChoRecordConfigurationException()
        {
            //Arrange
            var inputDataList = new List<DataETLTestModelDuplicateRecordField>
            {
                new DataETLTestModelDuplicateRecordField
                {
                    Field_Int1 = 1,
                    Field_Int2 = 2
                }
            };

            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            // Act
            // Assert
            Assert.Throws<ChoRecordConfigurationException>(() => csvFileTransformAdapter.GenerateByteArray(inputDataList));
        }

        [Fact]
        public void GenerateByteArray_DataETLTestModelMissingRecordField_ThrowsChoRecordConfigurationException()
        {
            //Arrange
            var inputDataList = new List<DataETLTestModelMissingRecordField>
            {
                new DataETLTestModelMissingRecordField
                {
                    Field_Int1 = 1,
                    Field_Int2 = 2
                }
            };

            var csvFileTransformAdapter = new CsvFileTransformAdapter();

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => csvFileTransformAdapter.GenerateByteArray(inputDataList));
        }
    }
}
