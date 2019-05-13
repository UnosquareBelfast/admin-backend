using System;
using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
  public interface ICsvService
  {
    /// <summary>
    /// Generate a csv from the given passed in data.
    /// Class type must be decorated with the required common attributes.
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Byte[] Generate<T>(IList<T> data) where T : class;
  }
}