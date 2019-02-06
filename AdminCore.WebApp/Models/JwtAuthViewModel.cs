using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models
{
  public class JwtAuthViewModel : ViewModel
  {
    public string AccessToken { get; set; }

    public string TokenType { get; set; }
  }
}