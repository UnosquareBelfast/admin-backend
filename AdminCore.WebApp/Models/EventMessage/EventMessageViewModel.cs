using AdminCore.WebApi.Models.Employee;
using System;
using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.EventMessage
{
  public class EventMessageViewModel : ViewModel
  {
    public int EventMessageId { get; set; }

    public int EventId { get; set; }

    public EmployeeViewModel Employee { get; set; }

    public string Message { get; set; }

    public DateTime LastModified { get; set; }

    public int EventMessageTypeId { get; set; }
  }
}