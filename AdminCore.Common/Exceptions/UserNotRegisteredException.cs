using System;

namespace AdminCore.WebApi.Exceptions
{
  public class UserNotRegisteredException : Exception
  {
    public UserNotRegisteredException(string message) : base(message){}
  }
}
