using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "E-mail")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Heslo")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Zapamätať si ma")]
    public bool RememberMe { get; set; }
    
    public string? DeviceToken { get; set; }
}
