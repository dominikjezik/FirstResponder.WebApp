using System.ComponentModel.DataAnnotations;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentFormDTO
{
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string Address { get; set; }

    public string? Patient { get; set; }

    [Required]
    public string Diagnosis { get; set; }

    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }
}