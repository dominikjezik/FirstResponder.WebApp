using Microsoft.Build.Framework;

namespace FirstResponder.Web.ViewModels;

public class IncidentNewMessageViewModel
{
    [Required]
    public string MessageContent { get; set; }
}