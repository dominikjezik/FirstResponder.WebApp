using FirstResponder.ApplicationCore.Entities.AedAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IAedLanguagesRepository
{
    Task<IEnumerable<Language>> GetAllLanguages();
    
    Task<Language?> GetLanguageById(Guid languageId);

    Task<bool> LanguageExists(string name);
    
    Task AddLanguage(Language language);
    
    Task UpdateLanguage(Language language);
    
    Task DeleteLanguage(Language language);
}