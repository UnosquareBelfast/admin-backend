using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class EventTypeViewModel : ViewModel
  {
    public int EventTypeId { get; set; }
    public string Description { get; set; }
  }
}