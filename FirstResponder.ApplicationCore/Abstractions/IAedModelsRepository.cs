using FirstResponder.ApplicationCore.Entities.AedAggregate;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IAedModelsRepository
{
    Task<IEnumerable<Model>> GetAllModels();
    
    Task<Model?> GetModelById(Guid modelId);

    Task<bool> ModelExists(string name);
    
    Task AddModel(Model model);
    
    Task UpdateModel(Model model);
    
    Task DeleteModel(Model model);
}