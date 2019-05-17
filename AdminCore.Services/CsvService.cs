using System.Collections.Generic;
using AdminCore.Common.Interfaces;

namespace AdminCore.Services
{
    public class CsvService : ICsvService
    {
        private readonly IFileTransformAdapter _fileTransformAdapter;
        
        public CsvService(IFileTransformAdapter fileTransformAdapter)
        {
            _fileTransformAdapter = fileTransformAdapter;
        }
        
        public byte[] Generate<T>(IList<T> data) where T : class
        {
            return _fileTransformAdapter.GenerateByteArray(data);
        }
    }
}