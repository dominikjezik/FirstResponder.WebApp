using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Aeds.DTOs;

public class CreateAedDTO
{
    [Display(Name = "Stav")]
    public AedState State { get; set; }
    
    [Display(Name = "Výrobca")]
    public Guid? ManufacturerId { get; set; }
    
    [Display(Name = "Model")]
    public Guid? ModelId { get; set; }
    
    [Display(Name = "Jazyk")]
    public Guid? LanguageId { get; set; }

    [Display(Name = "Sériové číslo")]
    public string? SerialNumber { get; set; }

    [Display(Name = "Typ")]
    public bool FullyAutomatic { get; set; }

    public bool ElectrodesAdults { get; set; }
    
    public bool ElectrodesChildren { get; set; }
    
    [Display(Name = "Expirácia elektród")]
    public DateTime ElectrodesAdultsExpiration { get; set; }

    [Display(Name = "Expirácia detských elektród")]
    public DateTime ElectrodesChildrenExpiration { get; set; }

    [Display(Name = "Expirácia batérií")]
    public DateTime BatteryExpiration { get; set; }
    
    [Display(Name = "Poznámky")]
    public string? Notes { get; set; }

    public Aed ToAed()
    {
        return new Aed
        {
            State = State,
            ManufacturerId = ManufacturerId,
            ModelId = ModelId,
            LanguageId = LanguageId,
            SerialNumber = SerialNumber,
            FullyAutomatic = FullyAutomatic,
            ElectrodesAdults = ElectrodesAdults,
            ElectrodesChildren = ElectrodesChildren,
            ElectrodesAdultsExpiration = ElectrodesAdultsExpiration,
            ElectrodesChildrenExpiration = ElectrodesChildrenExpiration,
            BatteryExpiration = BatteryExpiration,
            Notes = Notes
        };
    }
}