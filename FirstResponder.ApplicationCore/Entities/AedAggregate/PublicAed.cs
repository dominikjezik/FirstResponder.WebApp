using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class PublicAed : Aed
{
    [Required]
    public string? Holder { get; set; }
    
    public override string GetDisplayHolder() => Holder;
    
    [Required]
    public string? Address { get; set; }
    
    [Required]
    public string? City { get; set; }

    [Required]
    public string? PostalCode { get; set; }
    
    [Required]
    public RegionOfState Region { get; set; }
    
    [Range(-90, 90)]
    public double? Latitude { get; set; }

    [Range(-180, 180)]
    public double? Longitude { get; set; }

    public string? DescriptionLocation { get; set; }
    
    // Contact person fields
    
    public string? ContactPerson { get; set; }

    public string? ContactPersonPhone { get; set; }

    [EmailAddress]
    public string? ContactPersonEmail { get; set; }
    
    [Required]
    public PublicAedAvailability? Availability { get; set; }
    
    public ICollection<AedPhoto>? Photos { get; set; }
    
    public class PublicAedAvailability
    {
        [Required]
        public AedAvailabilityType Type { get; set; }
        
        public TimeSpan? TimeFrom { get; set; }
        
        public TimeSpan? TimeTo { get; set; }
        
        public DaysOfWeek? Days { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateTo { get; set; }
        
        public string? Description { get; set; }
    }
}