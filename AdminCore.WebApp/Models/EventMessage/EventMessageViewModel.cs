using AdminCore.WebApi.Models.Employee;
using System;
using AdminCore.WebApi.Models.Base;
using AdminCore.WebApi.Models.SystemUser;

namespace AdminCore.WebApi.Models.EventMessage
{
  public class EventMessageViewModel : ViewModel
  {
    public int EventMessageId { get; set; }

    public int EventId { get; set; }

    public int SystemUserId { get; set; }

    public SystemUserViewModel SystemUser { get; set; }

    public string Message { get; set; }

    public DateTime LastModified { get; set; }

    public int EventMessageTypeId { get; set; }
  }
}
