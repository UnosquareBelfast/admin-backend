using System;

namespace AdminCore.Common.Exceptions
{
  public class ValidationException : Exception
  {
    private const string ValidationError = "VALIDATION ERROR:";

    public ValidationException(string message) : base($"{ValidationError} {message}"){}
  }
}