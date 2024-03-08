using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Common.Enums;

public enum AedAvailabilityType
{
    [Display(Name = "Nepretržitá")]
    Anytime,
    
    [Display(Name = "Prevádzkové časy")]
    OperatingHours,
    
    [Display(Name = "V termíne")]
    InTerm,
    
    [Display(Name = "Iná")]
    Other
}