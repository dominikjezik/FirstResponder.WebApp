using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IAedRepository
{
    Task<IEnumerable<Aed>> GetAllAedsWithOwners();

    Task<IEnumerable<AedItemDTO>> GetAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<AedItemDTO>> GetPersonalAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<AedItemDTO>> GetPublicAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<AedItemDTO>> GetAllPublicAedItems();
    
    Task<Aed?> GetAedById(Guid id);

    Task AddAed(Aed aed);
    
    Task UpdateAed(Aed aed);

    Task DeleteAed(Aed aed);
    
    Task AddAedPhoto(AedPhoto aedPhoto);
    
    Task<ICollection<AedPhoto>> GetAedPhotos(Guid aedId);
    
    Task<ICollection<AedPhoto>> GetAedPhotosByIds(Guid aedId, string[] photosIdsForDelete);
    
    Task DeleteAedPhotosByIds(Guid aedId, string[] photosIdsForDelete);
    
    Task<IEnumerable<AedEventDTO>> GetAedEvents(DateTime from, DateTime to);
}