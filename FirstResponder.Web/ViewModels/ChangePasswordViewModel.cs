using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Aktuálne heslo")]
    public string CurrentPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Nové heslo")]
    public string NewPassword { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    [Display(Name = "Potvrdenie nového hesla")]
    public string ConfirmNewPassword { get; set; }
}