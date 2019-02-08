using System;
using System.Collections.Generic;
using System.Text;

namespace AdminCore.WebApi.Tests.Exceptions
{
  public class TestException : Exception
  {
    public TestException(string message) : base(message){}
  }
}
