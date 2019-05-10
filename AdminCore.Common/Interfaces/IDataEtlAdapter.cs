using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
    public interface IDataEtlAdapter
    {
        string ToCsv<T, TMappedType>(IList<T> data)
            where T : class
            where TMappedType : class;
    }
}