using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Common.Enums;

public enum IncidentState
{
    [Display(Name = "Vytvorený")]
    Created,
    
    [Display(Name = "Prebiehajúci")]
    InProgress,
    
    [Display(Name = "Ukončený")]
    Completed,
    
    [Display(Name = "Zrušený")]
    Canceled
}