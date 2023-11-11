using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Aeds.DTOs;

public class AedFormDTO
{
    #region Common properties
    
    public AedGeneralType GeneralType { get; set; }
    
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
    public DateTime? ElectrodesAdultsExpiration { get; set; }

    [Display(Name = "Expirácia detských elektród")]
    public DateTime? ElectrodesChildrenExpiration { get; set; }

    [Display(Name = "Expirácia batérií")]
    public DateTime? BatteryExpiration { get; set; }
    
    [Display(Name = "Poznámky")]
    public string? Notes { get; set; }
    
    #endregion

    #region Public Aed properties

    public string? Holder { get; set; }

    #endregion

    #region Personal Aed properties

    public Guid? OwnerId { get; set; }

    #endregion

    #region Edit form properties

    public Guid AedId { get; set; }

    public DateTime CreatedAt { get; set; }

    #endregion
    
    public Aed ToAed(Aed? targetAed = null)
    {
        Aed? aed = null;

        if (targetAed == null)
        {
            if (GeneralType == AedGeneralType.Public)
            {
                aed = new PublicAed();
            }
            else
            {
                aed = new PersonalAed();
            }
        }
        else
        {
            aed = targetAed;
        }
        
        if (GeneralType == AedGeneralType.Public)
        {
            var publicAed = (PublicAed)aed;
            publicAed.Holder = Holder;
        }
        else
        {
            var personalAed = (PersonalAed)aed;
            personalAed.OwnerId = OwnerId;
        }
        
        aed.State = State;
        aed.ManufacturerId = ManufacturerId;
        aed.ModelId = ModelId;
        aed.LanguageId = LanguageId;
        aed.SerialNumber = SerialNumber;
        aed.FullyAutomatic = FullyAutomatic;
        aed.ElectrodesAdults = ElectrodesAdults;
        aed.ElectrodesChildren = ElectrodesChildren;
        aed.ElectrodesAdultsExpiration = ElectrodesAdultsExpiration;
        aed.ElectrodesChildrenExpiration = ElectrodesChildrenExpiration;
        aed.BatteryExpiration = BatteryExpiration;
        aed.Notes = Notes;

        return aed;
    }
}

public static class AedExtensions
{
    public static AedFormDTO ToAedFormDTO(this Aed aed)
    {
        var dto = new AedFormDTO
        {
            State = aed.State,
            ManufacturerId = aed.ManufacturerId,
            ModelId = aed.ModelId,
            LanguageId = aed.LanguageId,
            SerialNumber = aed.SerialNumber,
            FullyAutomatic = aed.FullyAutomatic,
            ElectrodesAdults = aed.ElectrodesAdults,
            ElectrodesChildren = aed.ElectrodesChildren,
            ElectrodesAdultsExpiration = aed.ElectrodesAdultsExpiration,
            ElectrodesChildrenExpiration = aed.ElectrodesChildrenExpiration,
            BatteryExpiration = aed.BatteryExpiration,
            Notes = aed.Notes,
            AedId = aed.Id,
            CreatedAt = aed.CreatedAt
        };

        if (aed is PersonalAed personalAed)
        {
            dto.GeneralType = AedGeneralType.Personal;
            dto.OwnerId = personalAed.OwnerId;
        }
        else
        {
            var publicAed = (PublicAed)aed;
            dto.GeneralType = AedGeneralType.Public;
            dto.Holder = publicAed.Holder;
        }
        
        // TODO: ďalšie vlastnosti špecifické pre daný typ aed (osobné/verejné)

        return dto;
    }
}