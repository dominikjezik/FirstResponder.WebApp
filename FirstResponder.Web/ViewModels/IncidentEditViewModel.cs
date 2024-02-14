using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;

namespace FirstResponder.Web.ViewModels;

public class IncidentEditViewModel
{
    public Guid IncidentId { get; set; }
    
    public IncidentFormDTO IncidentForm { get; set; }

    public IncidentState State { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
}

public static class IncidentExtensions
{
    public static IncidentEditViewModel ToIncidentEditViewModel(this Incident incident)
    {
        return new IncidentEditViewModel
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
    
    public static IncidentEditViewModel ToIncidentEditViewModel(this Incident incident, IncidentFormDTO incidentForm)
    {
        return new IncidentEditViewModel
        {
            IncidentId = incident.Id,
            IncidentForm = incidentForm,
            State = incident.State,
            CreatedAt = incident.CreatedAt,
            ResolvedAt = incident.ResolvedAt
        };
    }
}
