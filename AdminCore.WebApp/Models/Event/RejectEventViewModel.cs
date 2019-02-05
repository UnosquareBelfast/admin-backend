using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class RejectEventViewModel : ViewModel
  {
    public int EventId { get; set; }

    public string Message { get; set; }
  }
}