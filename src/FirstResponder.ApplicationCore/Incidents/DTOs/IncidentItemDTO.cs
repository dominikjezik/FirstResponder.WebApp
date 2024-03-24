using FirstResponder.ApplicationCore.Common.Extensions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentItemDTO
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
    
    public string Address { get; set; }
    
    public string? Patient { get; set; }
    
    public string Diagnosis { get; set; }
    
    public string State { get; set; }
    
    public string DisplayState { get; set; }
    
    public double? Latitude { get; set; }
    
    public double? Longitude { get; set; }
}

public static partial class IncidentExtensions
{
    public static IncidentItemDTO ToItemDTO(this Incident incident)
    {
        return new IncidentItemDTO
        {
            Id = incident.Id,
            CreatedAt = incident.CreatedAt,
            ResolvedAt = incident.ResolvedAt,
            Address = incident.Address,
            Patient = incident.Patient,
            Diagnosis = incident.Diagnosis,
            State = incident.State.ToString(),
            DisplayState = incident.State.GetDisplayAttributeValue(),
            Latitude = incident.Latitude,
            Longitude = incident.Longitude
        };
    }
    
    public static IncidentItemForResponderDTO ToItemForResponderDTO(this Incident incident, IncidentResponder? incidentResponder = null)
    {
        return new IncidentItemForResponderDTO
        {
            Id = incident.Id,
            CreatedAt = incident.CreatedAt,
            ResolvedAt = incident.ResolvedAt,
            Address = incident.Address,
            Patient = incident.Patient,
            Diagnosis = incident.Diagnosis,
            State = incident.State.ToString(),
            DisplayState = incident.State.GetDisplayAttributeValue(),
            Latitude = incident.Latitude,
            Longitude = incident.Longitude,
            AcceptedAt = incidentResponder?.AcceptedAt,
            DeclinedAt = incidentResponder?.DeclinedAt,
        };
    }
}