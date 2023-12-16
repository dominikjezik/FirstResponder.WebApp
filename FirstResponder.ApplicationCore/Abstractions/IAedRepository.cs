using FirstResponder.ApplicationCore.Entities.AedAggregate;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IAedRepository
{
    Task<IEnumerable<Aed>> GetAllAedsWithOwners();
    
    Task<IEnumerable<PublicAed>> GetAllPublicAeds();
    
    Task<Aed?> GetAedById(Guid id);

    Task AddAed(Aed aed);
    
    Task UpdateAed(Aed aed);

    Task DeleteAed(Aed aed);
    
    Task AddAedPhoto(AedPhoto aedPhoto);
    
    Task<ICollection<AedPhoto>> GetAedPhotos(Guid aedId);
    
    Task DeleteAedPhotosByIds(Guid aedId, string[] photosIdsForDelete);
    
}
