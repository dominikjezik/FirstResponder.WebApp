using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class AedModelsRepository : IAedModelsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AedModelsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Model>> GetAllModels()
    {
        return await _dbContext.AedModels
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Model>> GetAllModelsByManufacturerId(Guid manufacturerId)
    {
        return await _dbContext.AedModels
            .Where(m => m.ManufacturerId == manufacturerId)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Model?> GetModelById(Guid modelId)
    {
        return await _dbContext.AedModels
            .Where(m => m.Id == modelId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> ModelExists(string name, Guid manufacturerId)
    {
        return await _dbContext.AedModels
            .Where(m => m.Name == name && m.ManufacturerId == manufacturerId)
            .AnyAsync();
    }

    public async Task AddModel(Model model)
    {
        _dbContext.AedModels.Add(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateModel(Model model)
    {
        _dbContext.Update(model);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteModel(Model model)
    {
        _dbContext.AedModels.Remove(model);
        await _dbContext.SaveChangesAsync();
    }
}