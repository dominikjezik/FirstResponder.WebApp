using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Common.Enums;

public enum AedState
{
    [Display(Name="Registrovaný")]
    Registered,
    
    [Display(Name="Pripravený")]
    Ready,
    
    [Display(Name = "Nepripravený")]
    NotReady,
    
    [Display(Name = "Mimo prevádzky")]
    OutOfService,
    
    [Display(Name = "Servisný zásah")]
    ServiceRequired,
    
    [Display(Name = "Zrušený")]
    Cancelled
}