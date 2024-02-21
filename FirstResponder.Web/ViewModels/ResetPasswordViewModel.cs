using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class ResetPasswordViewModel
{
    public string Token { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Nové heslo")]
    public string NewPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "'Nové heslo' a 'Potvrdenie nového hesla' sa nezhodujú.")]
    [Display(Name = "Potvrdenie nového hesla")]
    public string ConfirmNewPassword { get; set; }
}