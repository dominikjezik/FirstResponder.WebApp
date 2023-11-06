using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class LoginViewModel
{
    [Display(Name = "E-mail")]
    public required string Email { get; set; }

    [Display(Name = "Heslo")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Zapamätať si ma")]
    public bool RememberMe { get; set; }
}
