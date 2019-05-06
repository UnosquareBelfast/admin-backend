using System;

namespace AdminCore.Common.Exceptions
{
  public class WorkflowException : Exception
  {
    private const string WorkflowError = "WORKFLOW ERROR:";

    public WorkflowException(string message) : base($"{WorkflowError} {message}"){}
  }
}