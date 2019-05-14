using System;
using System.Collections;
using System.Collections.Generic;
using AdminCore.DataETL.Tests.Models;

namespace AdminCore.DataETL.Tests.ClassData
{
    public class CsvChoEtlAdapterClassData
    {
        public class GenerateByteArray_DataETLTestModelSimpleWithValidData_ValidByteArrayReturned_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // DataETLTestModelSimple, expectedCsv
                yield return new object[]
                {
                    new List<DataETLTestModelSimple>
                    {
                        new DataETLTestModelSimple
                        {
                            Field_Int = 1,
                            Field_Bool = true,
                            Field_Float = 2.5f,
                            Field_String = "testString",
                            Field_DateTime = new DateTime()
                        }   
                    },
                    "Field1,Field2,Field3,Field4,Field5\r\n" +
                    "1,01/01/0001 00:00:00,testString,True,2.5"
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        
        public class GenerateByteArray_DataETLTestModelSimpleDuplicateRecordField_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // DataETLTestModelSimple, expectedCsv
                yield return new object[]
                {
                    new List<DataETLTestModelSimpleDuplicateRecordField>
                    {
                        new DataETLTestModelSimpleDuplicateRecordField
                        {
                            Field_Int1 = 1,
                            Field_Int2 = 2
                        }   
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
