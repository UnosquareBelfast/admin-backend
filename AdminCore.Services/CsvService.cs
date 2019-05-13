using System.Collections.Generic;
using AdminCore.Common.Interfaces;

namespace AdminCore.Services
{
    public class CsvService : ICsvService
    {
        private readonly IDataEtlAdapter _dataEtlAdapter;
        
        public CsvService(IDataEtlAdapter dataEtlAdapter)
        {
            _dataEtlAdapter = dataEtlAdapter;
        }
        
        public byte[] Generate<T>(IList<T> data) where T : class
        {
            return _dataEtlAdapter.GenerateByteArray(data);
        }
    }
}