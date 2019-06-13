using AdminCore.DTOs.Employee;
using System;
using AdminCore.DTOs.SystemUser;

namespace AdminCore.DTOs.EventMessage
{
  public class EventMessageDto
  {
    public int EventMessageId { get; set; }

    public int EventId { get; set; }

    public int SystemUserId { get; set; }

    public SystemUserDto SystemUser { get; set; }

    public string Message { get; set; }

    public DateTime LastModified { get; set; }

    public int EventMessageTypeId { get; set; }
  }
}
