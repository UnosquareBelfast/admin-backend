using System;
using System.Collections;
using System.Collections.Generic;
using AdminCore.DataETL.Tests.Models;

namespace AdminCore.DataETL.Tests.ClassData
{
    public class CsvChoEtlAdapterClassData
    {
        public class GenerateByteArray_DataETLTestModelSimpleWithValidData_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: inputDataList: List<DataETLTestModelSimple>, expectedCsv: string
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
                    CsvChoEtlAdapterConstants.PrimitivesDataCsv1
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
                    CsvChoEtlAdapterConstants.PrimitivesDataCsv2
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class GenerateByteArray_DataETLTestModelTypeToStringConverter_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: inputDataList: List<DataETLTestModelSimple>, expectedCsv: string
                yield return new object[]
                {
                    new List<DataETLTestModelTypeToStringConverter>
                    {
                        new DataETLTestModelTypeToStringConverter
                        {
                            DataETLTestEnum1 = 2
                        }
                    },
                    CsvChoEtlAdapterConstants.EnumDataCsv1
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class GenerateByteArray_DataETLTestModelListToStringConverter_ReturnedByteArrayMatchesExpected_ClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                // ARGS: inputDataList: List<DataETLTestModelSimple>, expectedCsv: string
                yield return new object[]
                {
                    new List<DataETLTestModelListToStringConverter>
                    {
                        new DataETLTestModelListToStringConverter
                        {
                            StringCollection = new List<string> { "A", "B", "C" }
                        }
                    },
                    CsvChoEtlAdapterConstants.ListStringDataCsv1
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
