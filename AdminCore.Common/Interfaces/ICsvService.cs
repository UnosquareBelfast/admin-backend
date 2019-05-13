using System;
using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
  public interface ICsvService
  {
    Byte[] Generate<T>(IList<T> data) where T : class;
  }
}