using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IIncidentsRepository
{
    Task<IEnumerable<Incident>> GetIncidents();

    Task<IEnumerable<Incident>> GetOpenedIncidentsNearby(double latitude, double longitude, double radius, Guid? userId = null);
    
    Task<IEnumerable<IncidentItemDTO>> GetIncidentItems(int pageNumber, int pageSize, IncidentItemFiltersDTO? filtersDTO = null);
    
    Task<Incident?> GetIncidentById(Guid incidentId);

    Task<IncidentDTO?> GetIncidentDetailsById(Guid incidentId);
    
    Task AddIncident(Incident incident);
    
    Task UpdateIncident(Incident incident);
    
    Task DeleteIncident(Incident incident);
    
    Task AssignResponderToIncidents(Guid responderId, IEnumerable<Incident> incidents);
}