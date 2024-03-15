using System.ComponentModel.DataAnnotations;

namespace FirstResponder.Web.ViewModels;

public class NotificationViewModel
{
    [Required]
    public string Content { get; set; }
}