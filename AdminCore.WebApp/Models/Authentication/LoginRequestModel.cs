using AdminCore.WebApi.Models.Base;

namespace AdminCore.WebApi.Models.Authentication
{
  public class LoginRequestModel : ViewModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}