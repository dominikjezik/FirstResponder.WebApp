using FirstResponder.ApplicationCore.Entities.AedAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IAedManufacturersRepository
{
    Task<IEnumerable<Manufacturer>> GetAllManufacturers();
    
    Task<Manufacturer?> GetManufacturerById(Guid manufacturerId);

    Task<bool> ManufacturerExists(string name);
    
    Task AddManufacturer(Manufacturer manufacturer);
    
    Task UpdateManufacturer(Manufacturer manufacturer);
    
    Task DeleteManufacturer(Manufacturer manufacturer);
}