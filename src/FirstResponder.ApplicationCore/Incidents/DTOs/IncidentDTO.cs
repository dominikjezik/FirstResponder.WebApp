using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentDTO
{
    public Guid IncidentId { get; set; }
    
    public IncidentFormDTO IncidentForm { get; set; }

    public IncidentState State { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
}

public static partial class IncidentExtensions
{
    public static IncidentDTO ToDTO(this Incident incident)
    {
        return new IncidentDTO
        {
            IncidentId = incident.Id,
            IncidentForm = new IncidentFormDTO
            {
                Address = incident.Address,
                Patient = incident.Patient,
                Diagnosis = incident.Diagnosis,
                Latitude = incident.Latitude,
                Longitude = incident.Longitude
            },
            State = incident.State,
            CreatedAt = incident.CreatedAt,
            ResolvedAt = incident.ResolvedAt
        };
    }
}