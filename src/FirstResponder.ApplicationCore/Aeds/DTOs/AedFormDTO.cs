using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Aeds.DTOs;

public class AedFormDTO
{
    #region Common properties
    
    public AedGeneralType GeneralType { get; set; }
    
    [Display(Name = "Stav")]
    [EnumDataType(typeof(AedState), ErrorMessage = "Neplatný stav AED")]
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
    
    public string? Address { get; set; }
    
    public string? City { get; set; }

    public string? PostalCode { get; set; }
    
    public RegionOfState Region { get; set; }
    
    [Range(-90, 90)]
    public double? Latitude { get; set; }

    [Range(-180, 180)]
    public double? Longitude { get; set; }

    public string? DescriptionLocation { get; set; }
    
    // Availability fields
    
    public AedAvailabilityType AvailabilityType { get; set; }
    
    public TimeSpan? AvailabilityTimeFrom { get; set; }
    
    public TimeSpan? AvailabilityTimeTo { get; set; }
    
    public bool AvailabilityMonday { get; set; }
    public bool AvailabilityTuesday { get; set; }
    public bool AvailabilityWednesday { get; set; }
    public bool AvailabilityThursday { get; set; }
    public bool AvailabilityFriday { get; set; }
    public bool AvailabilitySaturday { get; set; }
    public bool AvailabilitySunday { get; set; }
    
    public DateTime? AvailabilityDateFrom { get; set; }
    
    public DateTime? AvailabilityDateTo { get; set; }
    
    public string? AvailabilityDescription { get; set; }
    
    // Contact person fields

    public string? ContactPerson { get; set; }

    public string? ContactPersonPhone { get; set; }

    [EmailAddress]
    public string? ContactPersonEmail { get; set; }
    
    public IList<FileUploadDto>? AedPhotoFileUploadDTOs { get; set; }

    public ICollection<AedPhoto>? Photos { get; set; }

    #endregion

    #region Personal Aed properties

    public Guid? OwnerId { get; set; }

    #endregion

    #region Edit form properties

    public Guid AedId { get; set; }

    public User? Owner { get; set; }

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
            if (aed is PublicAed)
            {
                GeneralType = AedGeneralType.Public;
            }
            else
            {
                GeneralType = AedGeneralType.Personal;
            }
        }
        
        if (GeneralType == AedGeneralType.Public)
        {
            var publicAed = (PublicAed)aed;
            publicAed.Holder = Holder;
            publicAed.Address = Address;
            publicAed.City = City;
            publicAed.PostalCode = PostalCode;
            publicAed.Region = Region;
            publicAed.Latitude = Latitude;
            publicAed.Longitude = Longitude;
            publicAed.DescriptionLocation = DescriptionLocation;
            
            // Contact person fields
            publicAed.ContactPerson = ContactPerson;
            publicAed.ContactPersonPhone = ContactPersonPhone;
            publicAed.ContactPersonEmail = ContactPersonEmail;
            
            // Availability fields
            if (publicAed.Availability == null)
            {
                publicAed.Availability = new PublicAed.PublicAedAvailability();
            }

            publicAed.Availability.Type = AvailabilityType;
            publicAed.Availability.TimeFrom = AvailabilityTimeFrom;
            publicAed.Availability.TimeTo = AvailabilityTimeTo;
            publicAed.Availability.Days = (AvailabilityMonday ? DaysOfWeek.Monday : DaysOfWeek.None) |
                                          (AvailabilityTuesday ? DaysOfWeek.Tuesday : DaysOfWeek.None) |
                                          (AvailabilityWednesday ? DaysOfWeek.Wednesday : DaysOfWeek.None) |
                                          (AvailabilityThursday ? DaysOfWeek.Thursday : DaysOfWeek.None) |
                                          (AvailabilityFriday ? DaysOfWeek.Friday : DaysOfWeek.None) |
                                          (AvailabilitySaturday ? DaysOfWeek.Saturday : DaysOfWeek.None) |
                                          (AvailabilitySunday ? DaysOfWeek.Sunday : DaysOfWeek.None);
            publicAed.Availability.DateFrom = AvailabilityDateFrom;
            publicAed.Availability.DateTo = AvailabilityDateTo;
            publicAed.Availability.Description = AvailabilityDescription;
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
            dto.Owner = personalAed?.Owner;
        }
        else
        {
            var publicAed = (PublicAed)aed;
            dto.GeneralType = AedGeneralType.Public;
            dto.Holder = publicAed.Holder;
            dto.Address = publicAed.Address;
            dto.City = publicAed.City;
            dto.PostalCode = publicAed.PostalCode;
            dto.Region = publicAed.Region;
            dto.Latitude = publicAed.Latitude;
            dto.Longitude = publicAed.Longitude;
            dto.DescriptionLocation = publicAed.DescriptionLocation;
            
            // Contact person fields
            dto.ContactPerson = publicAed.ContactPerson;
            dto.ContactPersonPhone = publicAed.ContactPersonPhone;
            dto.ContactPersonEmail = publicAed.ContactPersonEmail;
            
            // Availability fields
            dto.AvailabilityType = publicAed.Availability?.Type ?? AedAvailabilityType.Anytime;
            dto.AvailabilityTimeFrom = publicAed.Availability?.TimeFrom;
            dto.AvailabilityTimeTo = publicAed.Availability?.TimeTo;
            
            // Days
            dto.AvailabilityMonday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Monday) ?? false;
            dto.AvailabilityTuesday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Tuesday) ?? false;
            dto.AvailabilityWednesday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Wednesday) ?? false;
            dto.AvailabilityThursday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Thursday) ?? false;
            dto.AvailabilityFriday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Friday) ?? false;
            dto.AvailabilitySaturday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Saturday) ?? false;
            dto.AvailabilitySunday = publicAed.Availability?.Days?.HasFlag(DaysOfWeek.Sunday) ?? false;
            
            dto.AvailabilityDateFrom = publicAed.Availability?.DateFrom;
            dto.AvailabilityDateTo = publicAed.Availability?.DateTo;
            dto.AvailabilityDescription = publicAed.Availability?.Description;
            
            dto.Photos = publicAed.Photos;
        }

        return dto;
    }
}