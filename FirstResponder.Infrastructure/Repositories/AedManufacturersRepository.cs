using FirstResponder.ApplicationCore.Abstractions;
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
            .OrderByDescending(m => m.Name)
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
        _dbContext.AedManufacturers.Remove(manufacturer);
        await _dbContext.SaveChangesAsync();
    }
}