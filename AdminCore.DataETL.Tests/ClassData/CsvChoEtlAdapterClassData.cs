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
                // List<DataETLTestModelSimple>, expectedCsv
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
                yield return new object[]
                {
                    new List<DataETLTestModelSimple>
                    {
                        new DataETLTestModelSimple
                        {
                            Field_Int = -10,
                            Field_Bool = false,
                            Field_Float = 19239381f,
                            Field_String = "2(*10=3@~}?><!\"£$\"%^&*()",
                            Field_DateTime = new DateTime(1970, 1, 1)
                        }   
                    },
                    "Field1,Field2,Field3,Field4,Field5\r\n" +
                    "-10,01/01/1970 00:00:00,\"2(*10=3@~}?><!\"\"\"\"£$\"\"\"\"%^&*()\",False,1.923938E+07"
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        
        public class GenerateByteArray_DataETLTestModelTypeToStringConverter_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // List<DataETLTestModelTypeToStringConverter>, expectedCsv
                yield return new object[]
                {
                    new List<DataETLTestModelTypeToStringConverter>
                    {
                        new DataETLTestModelTypeToStringConverter
                        {
                            DataETLTestEnum1 = 2
                        }   
                    },
                    "Field1\r\ncEnum"
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        
        public class GenerateByteArray_DataETLTestModelListToStringConverter_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // List<DataETLTestModelListToStringConverter>, expectedCsv
                yield return new object[]
                {
                    new List<DataETLTestModelListToStringConverter>
                    {
                        new DataETLTestModelListToStringConverter
                        {
                            StringCollection = new List<string> { "A", "B", "C" }
                        }   
                    },
                    "Field1\r\n\"A\r\nB\r\nC\""
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
