using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}