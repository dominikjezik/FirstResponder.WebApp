using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class AedManufacturersRepository : IAedManufacturersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AedManufacturersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Manufacturer>> GetAllManufacturers()
    {
        return await _dbContext.AedManufacturers
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<Manufacturer?> GetManufacturerById(Guid manufacturerId)
    {
        return await _dbContext.AedManufacturers
            .Where(m => m.Id == manufacturerId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> ManufacturerExists(string name)
    {
        return await _dbContext.AedManufacturers
            .Where(m => m.Name == name)
            .AnyAsync();
    }

    public async Task AddManufacturer(Manufacturer manufacturer)
    {
        _dbContext.AedManufacturers.Add(manufacturer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateManufacturer(Manufacturer manufacturer)
    {
        _dbContext.Update(manufacturer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteManufacturer(Manufacturer manufacturer)
    {
        // The transaction is used to delete the manufacturer and its models.
        // It is a problem to define cascading deletion so transaction is used.
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        await _dbContext.AedModels
            .Where(m => m.ManufacturerId == manufacturer.Id)
            .ExecuteDeleteAsync();
        
        _dbContext.AedManufacturers.Remove(manufacturer);
        
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}