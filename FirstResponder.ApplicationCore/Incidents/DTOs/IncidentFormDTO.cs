using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentFormDTO
{
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

    public Incident ToIncident(Incident? targetIncident = null)
    {
        Incident? incident = null;

        if (targetIncident == null)
        {
            incident = new Incident();
        }
        else
        {
            incident = targetIncident;
        }

        incident.Address = Address;
        incident.Patient = Patient;
        incident.Diagnosis = Diagnosis;
        incident.Latitude = Latitude;
        incident.Longitude = Longitude;

        return incident;
    }
}