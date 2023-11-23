using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Enums;

public enum UserType
{
    [Display(Name="Nezaradený")]
    Default,
    
    [Display(Name="Responder")]
    Responder,
    
    [Display(Name="Zamestnanec")]
    Employee
}