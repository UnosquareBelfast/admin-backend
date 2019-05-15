namespace AdminCore.DataETL.Tests.ClassData
{
    public static class CsvChoEtlAdapterConstants
    {
        public static readonly string PrimitivesDataCsv1 = "Field1,Field2,Field3,Field4,Field5\r\n1,01/01/0001 00:00:00,testString,True,2.5";
        public static readonly string PrimitivesDataCsv2 = "Field1,Field2,Field3,Field4,Field5\r\n-10,01/01/1970 00:00:00,\"2(*10=3@~}?><!\"\"\"\"£$\"\"\"\"%^&*()\",False,1.923938E+07";
        public static readonly string EnumDataCsv1 = "Field1\r\ncEnum";
        public static readonly string ListStringDataCsv1 = "Field1\r\n\"A\r\nB\r\nC\"";
    }
}
