using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class AedRepository : IAedRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AedRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Aed>> GetAllAeds()
    {
        return await _dbContext.Aeds
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Aed> AddAed(Aed aed)
    {
        _dbContext.Aeds.Add(aed);
        await _dbContext.SaveChangesAsync();

        return aed;
    }

    public async Task<IEnumerable<Manufacturer>> GetAllManufacturers()
    {
        return await _dbContext.AedManufacturers.ToListAsync();
    }

    public async Task<IEnumerable<Model>> GetAllModels()
    {
        return await _dbContext.AedModels.ToListAsync();
    }

    public async Task<IEnumerable<Language>> GetAllLanguages()
    {
        return await _dbContext.AedLanguages.ToListAsync();
    }
}
