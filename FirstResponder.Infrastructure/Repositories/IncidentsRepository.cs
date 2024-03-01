using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Extensions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class IncidentsRepository : IIncidentsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IncidentsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<IncidentItemDTO>> GetIncidentItems(int pageNumber, int pageSize, IncidentItemFiltersDTO? filtersDTO = null)
    {
        var query = _dbContext.Incidents
            .OrderByDescending(i => i.CreatedAt)
            .AsQueryable();

        if (filtersDTO != null)
        {
            query = query
                .Where( i =>
                    (filtersDTO.From == null || i.CreatedAt >= filtersDTO.From) &&
                    (filtersDTO.To == null || i.CreatedAt <= filtersDTO.To) &&
                    (string.IsNullOrEmpty(filtersDTO.Patient) || i.Patient.Contains(filtersDTO.Patient)) &&
                    (string.IsNullOrEmpty(filtersDTO.Address) || i.Address.Contains(filtersDTO.Address)) &&
                    (filtersDTO.State == null || filtersDTO.State == i.State)
                );
        }

        var queryItems = query
            .Select(i => new IncidentItemDTO
            {
                Id = i.Id,
                CreatedAt = i.CreatedAt,
                ResolvedAt = i.ResolvedAt,
                Patient = i.Patient,
                Address = i.Address,
                Diagnosis = i.Diagnosis,
                State = i.State.ToString(),
                DisplayState = i.State.GetDisplayAttributeValue(),
                Latitude = i.Latitude,
                Longitude = i.Longitude
            });

        if (pageSize > 0)
        {
            queryItems = queryItems
                .Skip(pageNumber * pageSize)
                .Take(pageSize);
        }
        
        return await queryItems.ToListAsync();
    }

    public async Task<IEnumerable<Incident>> GetOpenedIncidentsNearby(double latitude, double longitude, double radiusInMeters, Guid? userId = null)
    {
        var incidents = _dbContext.Incidents
            .Where(i => i.State == IncidentState.Created || i.State == IncidentState.InProgress);
        
        if (userId != null)
        {
            incidents = incidents
                .Include(i => i.Responders.Where(r => r.ResponderId == userId));
        }
        
        // Get incidents only in the specified radius using the Haversine formula
        // https://en.wikipedia.org/wiki/Haversine_formula
        incidents = incidents
            .Where(i =>
                2 * 6371 * Math.Asin(
                    Math.Sqrt(
                        Math.Pow(Math.Sin((latitude - i.Latitude) * (Math.PI / 180) / 2), 2) +
                        Math.Cos(latitude * (Math.PI / 180)) *
                        Math.Cos(i.Latitude * (Math.PI / 180)) *
                        Math.Pow(Math.Sin((longitude - i.Longitude) * (Math.PI / 180) / 2), 2)
                    )
                ) <= radiusInMeters / 1000);

        return await incidents.ToListAsync();
    }
    
    public async Task<IncidentDTO?> GetIncidentDetailsById(Guid incidentId)
    {
        return await _dbContext.Incidents
            .Where(i => i.Id == incidentId)
            .Include(i => i.Responders)
            .Select(i => new {
                Incident = i,
                Responders = i.Responders
                    .Where(r => r.AcceptedAt != null)
                    .Join(
                        _dbContext.Users,
                        r => r.ResponderId,
                        u => u.Id,
                        (r, u) => new { Responder = r, User = u }
                    ).ToList()
            })
            .Select(result => new IncidentDTO
            {
                IncidentId = result.Incident.Id,
                CreatedAt = result.Incident.CreatedAt,
                State = result.Incident.State,
                ResolvedAt = result.Incident.ResolvedAt,
                IncidentForm = new IncidentFormDTO
                {
                    Patient = result.Incident.Patient,
                    Address = result.Incident.Address,
                    Diagnosis = result.Incident.Diagnosis,
                    Latitude = result.Incident.Latitude,
                    Longitude = result.Incident.Longitude
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Incident?> GetIncidentById(Guid incidentId)
    {
        return await _dbContext.Incidents
            .Where(i => i.Id == incidentId)
            .FirstOrDefaultAsync();
    }

    public async Task AddIncident(Incident incident)
    {
        _dbContext.Incidents.Add(incident);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateIncident(Incident incident)
    {
        _dbContext.Update(incident);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteIncident(Incident incident)
    {
        _dbContext.Incidents.Remove(incident);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IncidentResponderItemDTO>> GetIncidentResponders(Guid incidentId)
    {
        return await _dbContext.IncidentResponders
            .Where(r => r.IncidentId == incidentId)
            .Where(r => r.AcceptedAt != null)
            .Join(
                _dbContext.Users,
                r => r.ResponderId,
                u => u.Id,
                (r, u) => new IncidentResponderItemDTO
                {
                    ResponderId = r.ResponderId,
                    FullName = u.FullName,
                    AcceptedAt = r.AcceptedAt,
                }
            )
            .ToListAsync();
    }

    public async Task AssignResponderToIncidents(Guid responderId, IEnumerable<Incident> incidents)
    {
        // Filter out incidents that have already been assigned to a responder
        incidents = incidents
            .Where(i => !i.Responders.Any(r => r.ResponderId == responderId));
        
        foreach (var incident in incidents)
        {
            var responderIncident = new IncidentResponder
            {
                ResponderId = responderId,
                IncidentId = incident.Id,
                CreatedAt = DateTime.Now
            };
            
            incident.Responders.Add(responderIncident);
            _dbContext.IncidentResponders.Add(responderIncident);
        }
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IncidentResponder> AcceptIncident(Incident incident, User user)
    {
        var responderIncident = await _dbContext.IncidentResponders
            .Where(r => r.IncidentId == incident.Id && r.ResponderId == user.Id)
            .FirstOrDefaultAsync();
        
        if (responderIncident == null)
        {
            responderIncident = new IncidentResponder
            {
                ResponderId = user.Id,
                IncidentId = incident.Id,
                CreatedAt = DateTime.Now
            };
            
            incident.Responders.Add(responderIncident);
            _dbContext.IncidentResponders.Add(responderIncident);
        }
        
        responderIncident.AcceptedAt = DateTime.Now;
        responderIncident.DeclinedAt = null;
        
        await _dbContext.SaveChangesAsync();
        return responderIncident;
    }

    public async Task DeclineIncident(Incident incident, User user)
    {
        var responderIncident = await _dbContext.IncidentResponders
            .Where(r => r.IncidentId == incident.Id && r.ResponderId == user.Id)
            .FirstOrDefaultAsync();
        
        if (responderIncident == null)
        {
            responderIncident = new IncidentResponder
            {
                ResponderId = user.Id,
                IncidentId = incident.Id,
                CreatedAt = DateTime.Now
            };
            
            incident.Responders.Add(responderIncident);
            _dbContext.IncidentResponders.Add(responderIncident);
        }
        
        responderIncident.DeclinedAt = DateTime.Now;
        responderIncident.AcceptedAt = null;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<IncidentMessageDTO>> GetIncidentMessages(Guid incidentId)
    {
        return await _dbContext.IncidentMessages
            .Where(m => m.IncidentId == incidentId)
            .OrderByDescending(m => m.CreatedAt)
            .Join(
                _dbContext.Users,
                m => m.SenderId,
                u => u.Id,
                (r, u) => new { Message = r, User = u }
            )
            .Select(result => new IncidentMessageDTO
            {
                Id = result.Message.Id,
                CreatedAt = result.Message.CreatedAt,
                Content = result.Message.Content,
                SenderName = result.User.FullName
            }).ToListAsync();
    }

    public async Task<IncidentMessageDTO> SendMessageToIncident(Incident incident, User user, string message)
    {
        var incidentMessage = new IncidentMessage
        {
            IncidentId = incident.Id,
            SenderId = user.Id,
            Content = message,
            CreatedAt = DateTime.Now
        };
        
        _dbContext.IncidentMessages.Add(incidentMessage);
        await _dbContext.SaveChangesAsync();
        
        return new IncidentMessageDTO
        {
            Id = incidentMessage.Id,
            CreatedAt = incidentMessage.CreatedAt,
            Content = incidentMessage.Content,
            SenderName = user.FullName
        };
    }
}