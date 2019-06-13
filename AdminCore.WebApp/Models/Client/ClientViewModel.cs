using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Client
{
  public class ClientViewModel : ViewModel
  {
    public int ClientId { get; set; }

    public int SystemUserId { get; set; }

    public string ClientName { get; set; }
  }
}
