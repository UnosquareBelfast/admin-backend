using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Client
{
  public class UpdateClientViewModel : ViewModel
  {
    public int ClientId { get; set; }

    public string ClientName { get; set; }
  }
}