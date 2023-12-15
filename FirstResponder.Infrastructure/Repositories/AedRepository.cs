using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class AedRepository : IAedRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AedRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Aed>> GetAllAedsWithOwners()
    {
        var aeds =  await _dbContext.Aeds
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var personalAeds = aeds.OfType<PersonalAed>().ToList();
        
        var owners = (await _dbContext.Users
            .Where(u => personalAeds.Select(a => a.OwnerId).Contains(u.Id))
            .ToListAsync())
            .Select(user => user.ToDomainUser())
            .ToList();
        
        foreach (var personalAed in personalAeds)
        {
            personalAed.Owner = owners.Find(u => u.Id == personalAed.OwnerId);
        }
        
        return aeds;
    }
    
    public async Task<IEnumerable<PublicAed>> GetAllPublicAeds()
    {
        return await _dbContext.PublicAeds
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAed(Aed aed)
    {
        _dbContext.Aeds.Add(aed);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Aed?> GetAedById(Guid id)
    {
        return await _dbContext.Aeds.Where(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAed(Aed aed)
    {
        _dbContext.Update(aed);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAed(Aed aed)
    {
        _dbContext.Aeds.Remove(aed);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddAedPhoto(AedPhoto aedPhoto)
    {
        _dbContext.Add(aedPhoto);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ICollection<AedPhoto>> GetAedPhotos(Guid aedId)
    {
        return await _dbContext.AedPhotos
            .Where(p => p.PublicAedId == aedId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAedPhotosByIds(Guid aedId, string[] photosIdsForDelete)
    {
        var photos =  await _dbContext.AedPhotos
            .Where(p => photosIdsForDelete.Contains(p.Id.ToString()))
            .ToListAsync();
        
        _dbContext.AedPhotos.RemoveRange(photos);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Language>> GetAllLanguages()
    {
        return await _dbContext.AedLanguages.ToListAsync();
    }
}
