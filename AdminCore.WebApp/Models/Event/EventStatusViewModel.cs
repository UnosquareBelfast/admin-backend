using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Event
{
  public class EventStatusViewModel : ViewModel
  {
    public int EventStatusId { get; set; }
    public string Description { get; set; }
  }
}