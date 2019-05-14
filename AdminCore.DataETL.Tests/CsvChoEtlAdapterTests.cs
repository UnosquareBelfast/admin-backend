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
    public class CsvChoEtlAdapterTests
    {
        [Fact]
        public void GenerateByteArray_NullUsedAsCsvDataSource_ReturnedByArrayIsEmptyNotNull()
        {
            //Arrange
            var csvChoEtlAdapter = new CsvChoEtlAdapter();
            
            // Act
            var csvResult = csvChoEtlAdapter.GenerateByteArray<DataETLTestModelSimple>(null);
            
            // Assert
            Assert.True(!csvResult.Any());
        }
        
        [Theory]
        [ClassData(typeof(CsvChoEtlAdapterClassData.GenerateByteArray_DataETLTestModelSimpleWithValidData_ValidByteArrayReturned_ClassData))]
        public void GenerateByteArray_DataETLTestModelSimpleWithValidData_ReturnedByteArrayMatchesExpected(
            IList<DataETLTestModelSimple> inputDataList, string expectedCsv)
        {
            //Arrange
            var csvChoEtlAdapter = new CsvChoEtlAdapter();
            
            // Act
            var csvResult = csvChoEtlAdapter.GenerateByteArray(inputDataList);
            var actualCsv = System.Text.Encoding.Default.GetString(csvResult);
            
            // Assert
            Assert.Equal(expectedCsv, actualCsv);
        }
        
        [Theory]
        [ClassData(typeof(CsvChoEtlAdapterClassData.GenerateByteArray_DataETLTestModelSimpleDuplicateRecordField_ReturnedByteArrayMatchesExpected_ClassData))]
        public void GenerateByteArray_DataETLTestModelSimpleDuplicateRecordField_ThrowsChoRecordConfigurationException(
            IList<DataETLTestModelSimpleDuplicateRecordField> inputDataList)
        {
            //Arrange
            var csvChoEtlAdapter = new CsvChoEtlAdapter();
            
            // Act
            // Assert
            Assert.Throws<ChoRecordConfigurationException>(() => csvChoEtlAdapter.GenerateByteArray(inputDataList));           
        }
    }
}
