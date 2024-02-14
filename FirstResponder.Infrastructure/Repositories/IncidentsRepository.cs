using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Extentions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
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
    
    public async Task<IEnumerable<Incident>> GetIncidents()
    {
        return await _dbContext.Incidents
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
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
        
        return await query
            .Select(i => new IncidentItemDTO
            {
                Id = i.Id,
                CreatedAt = i.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                Patient = i.Patient,
                Address = i.Address,
                Diagnosis = i.Diagnosis,
                State = i.State.GetDisplayAttributeValue()
            })
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
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
}