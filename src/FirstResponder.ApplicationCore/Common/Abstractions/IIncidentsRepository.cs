using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IIncidentsRepository
{
    Task<IEnumerable<IncidentItemDTO>> GetIncidentItems(int pageNumber, int pageSize, IncidentItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<Incident>> GetOpenedIncidentsNearby(double latitude, double longitude, double radiusInMeters, Guid? userId = null);
    
    Task<IEnumerable<IncidentDTO>> GetUserIncidents(Guid userId);
    
    Task<Incident?> GetIncidentById(Guid incidentId);

    Task<IncidentDTO?> GetIncidentDetailsById(Guid incidentId);
    
    Task<Incident?> GetIncidentWithRespondersById(Guid incidentId);
    
    Task AddIncident(Incident incident);
    
    Task UpdateIncident(Incident incident);
    
    Task DeleteIncident(Incident incident);
    
    Task<IEnumerable<IncidentResponderItemDTO>> GetIncidentResponders(Guid incidentId);
    
    Task<IncidentResponder?> GetIncidentResponder(Guid incidentId, Guid responderId);
    
    Task<IncidentReportDTO?> GetIncidentReport(Guid incidentId, Guid responderId);
    
    Task CreateOrUpdateIncidentReport(IncidentResponder incidentResponder, IncidentReport report);
    
    Task AssignResponderToIncidents(Guid responderId, IEnumerable<Incident> incidents);
    
    Task<IncidentResponder> AcceptIncident(Incident incident, User user, double? latitude = null, double? longitude = null, TypeOfResponderTransport? typeOfTransport = null);
    
    Task DeclineIncident(Incident incident, User user);
    
    Task<IEnumerable<IncidentMessageDTO>> GetIncidentMessages(Guid incidentId);
    
    Task<IncidentMessageDTO> SendMessageToIncident(Incident incident, User user, string message);
}