using System;

namespace AdminCore.WebApi.Tests.Exceptions
{
  public class TestException : Exception
  {
    public TestException(string message) : base(message){}
  }
}
