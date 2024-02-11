using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Common.Enums;

public enum UserType
{
    [Display(Name="Nezaradený")]
    Default,
    
    [Display(Name="Responder")]
    Responder,
    
    [Display(Name="Zamestnanec")]
    Employee
}