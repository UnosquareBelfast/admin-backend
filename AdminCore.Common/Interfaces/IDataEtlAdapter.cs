using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
    public interface IDataEtlAdapter
    {
        /// <summary>
        /// Generate a byte array from the given data.
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        byte[] GenerateByteArray<T>(IList<T> data)
            where T : class;
    }
}