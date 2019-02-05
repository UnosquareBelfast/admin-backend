using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.EventMessage
{
  public class CreateEventMessageViewModel : ViewModel
  {
    public int EventId { get; set; }
    public string Message { get; set; }
  }
}