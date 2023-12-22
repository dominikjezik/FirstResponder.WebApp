using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class AedLanguagesRepository : IAedLanguagesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AedLanguagesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Language>> GetAllLanguages()
    {
        return await _dbContext.AedLanguages
            .OrderByDescending(m => m.Name)
            .ToListAsync();
    }

    public async Task<Language?> GetLanguageById(Guid languageId)
    {
        return await _dbContext.AedLanguages
            .Where(m => m.Id == languageId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> LanguageExists(string name)
    {
        return await _dbContext.AedLanguages
            .Where(m => m.Name == name)
            .AnyAsync();
    }

    public async Task AddLanguage(Language language)
    {
        _dbContext.AedLanguages.Add(language);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateLanguage(Language language)
    {
        _dbContext.Update(language);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteLanguage(Language language)
    {
        _dbContext.AedLanguages.Remove(language);
        await _dbContext.SaveChangesAsync();
    }
}