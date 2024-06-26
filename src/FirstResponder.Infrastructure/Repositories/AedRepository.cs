using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Extensions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;
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

    public async Task<IEnumerable<AedItemDTO>> GetAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null)
    {
        var aedItemsQueryable = _dbContext.Aeds
            .Include(aed => aed.Manufacturer)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();
        
        if (filtersDTO != null)
        {
            aedItemsQueryable = aedItemsQueryable
                .Where(a =>
                    (filtersDTO.State == null || filtersDTO.State == a.State) &&
                    (filtersDTO.ManufacturerId == null || filtersDTO.ManufacturerId == a.ManufacturerId) &&
                    (filtersDTO.ModelId == null || filtersDTO.ModelId == a.ModelId) &&
                    (string.IsNullOrEmpty(filtersDTO.SerialNumber) || a.SerialNumber.Contains(filtersDTO.SerialNumber))
                );
        }
        
        var aeds = await aedItemsQueryable
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
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

        return aeds.Select(a => new AedItemDTO
        {
            Id = a.Id,
            State = a.State.ToString(),
            DisplayState = a.State.GetDisplayAttributeValue(),
            Holder = a.GetDisplayHolder(),
            CreatedAt = a.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
            SerialNumber = a.SerialNumber,
            Manufacturer = a.Manufacturer?.Name
        });
    }

    public async Task<IEnumerable<AedItemDTO>> GetPersonalAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null)
    {
        var aedItemsQueryable = _dbContext.PersonalAeds
            .Include(aed => aed.Manufacturer)
            .OrderByDescending(a => a.CreatedAt)
            .Join(
                _dbContext.Users,
                a => a.OwnerId,
                u => u.Id,
                (a, u) => new { Aed = a, User = u }
            );

        if (filtersDTO != null)
        {
            aedItemsQueryable = aedItemsQueryable
                .Where(a =>
                    (string.IsNullOrEmpty(filtersDTO.Holder) || a.User.FullName.Contains(filtersDTO.Holder)) &&
                    (filtersDTO.Region == null || filtersDTO.Region == a.User.Region) &&
                    (filtersDTO.State == null || filtersDTO.State == a.Aed.State) &&
                    (filtersDTO.ManufacturerId == null || filtersDTO.ManufacturerId == a.Aed.ManufacturerId) &&
                    (filtersDTO.ModelId == null || filtersDTO.ModelId == a.Aed.ModelId) &&
                    (string.IsNullOrEmpty(filtersDTO.SerialNumber) || a.Aed.SerialNumber.Contains(filtersDTO.SerialNumber))
                );
        }
            
        return await aedItemsQueryable
            .Select(a => new AedItemDTO()
            {
                Id = a.Aed.Id,
                State = a.Aed.State.ToString(),
                DisplayState = a.Aed.State.GetDisplayAttributeValue(),
                Holder = a.User.FullName,
                CreatedAt = a.Aed.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
                SerialNumber = a.Aed.SerialNumber,
                Manufacturer = a.Aed.Manufacturer.Name
            })
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<AedItemDTO>> GetPublicAedItems(int pageNumber, int pageSize, AedItemFiltersDTO? filtersDTO = null)
    {
        var aedItemsQueryable = _dbContext.PublicAeds
            .Include(aed => aed.Manufacturer)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();
        
        if (filtersDTO != null)
        {
            aedItemsQueryable = aedItemsQueryable
                .Where(a =>
                    (string.IsNullOrEmpty(filtersDTO.Holder) || a.Holder.Contains(filtersDTO.Holder)) &&
                    (filtersDTO.Region == null || filtersDTO.Region == a.Region) &&
                    (filtersDTO.State == null || filtersDTO.State == a.State) &&
                    (filtersDTO.ManufacturerId == null || filtersDTO.ManufacturerId == a.ManufacturerId) &&
                    (filtersDTO.ModelId == null || filtersDTO.ModelId == a.ModelId) &&
                    (string.IsNullOrEmpty(filtersDTO.SerialNumber) || a.SerialNumber.Contains(filtersDTO.SerialNumber))
                );
        }
        
        return await aedItemsQueryable
            .Select(a => new AedItemDTO()
            {
                Id = a.Id,
                State = a.State.ToString(),
                DisplayState = a.State.GetDisplayAttributeValue(),
                Holder = a.GetDisplayHolder(),
                CreatedAt = a.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
                SerialNumber = a.SerialNumber,
                Manufacturer = a.Manufacturer.Name
            })
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<AedItemDTO>> GetAllPublicAedItems()
    {
        return await _dbContext.PublicAeds
            .Include(aed => aed.Manufacturer)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new AedItemDTO()
            {
                Id = a.Id,
                State = a.State.ToString(),
                DisplayState = a.State.GetDisplayAttributeValue(),
                Holder = a.GetDisplayHolder(),
                CreatedAt = a.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
                SerialNumber = a.SerialNumber,
                Manufacturer = a.Manufacturer.Name,
                ManufacturerId = a.ManufacturerId,
                ModelId = a.ModelId,
                Region = a.Region,
                Latitude = a.Latitude,
                Longitude = a.Longitude
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<AedItemDTO>> GetPersonalAedItemsByOwnerId(Guid ownerId)
    {
        return await _dbContext.PersonalAeds
            .Include(aed => aed.Manufacturer)
            .OrderByDescending(a => a.CreatedAt)
            .Where(a => a.OwnerId == ownerId)
            .Join(
                _dbContext.Users,
                a => a.OwnerId,
                u => u.Id,
                (a, u) => new { Aed = a, User = u }
            ).Select(result => new AedItemDTO()
            {
                Id = result.Aed.Id,
                State = result.Aed.State.ToString(),
                DisplayState = result.User.FullName,
                Holder = result.User.FullName,
                CreatedAt = result.Aed.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
                SerialNumber = result.Aed.SerialNumber,
                Manufacturer = result.Aed.Manufacturer.Name
            }).ToListAsync();
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

    public async Task<ICollection<AedPhoto>> GetAedPhotosByIds(Guid aedId, string[] photosIdsForDelete)
    {
        return await _dbContext.AedPhotos
            .Where(p => p.PublicAedId == aedId)
            .Where(p => photosIdsForDelete.Contains(p.Id.ToString()))
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

    public async Task<IEnumerable<AedEventDTO>> GetAedEvents(DateTime from, DateTime to)
    {
        var publicAeds = await _dbContext.PublicAeds
            .Where(a => (a.BatteryExpiration <= to && a.BatteryExpiration >= from) ||
                (a.ElectrodesAdultsExpiration <= to && a.ElectrodesAdultsExpiration >= from) ||
                (a.ElectrodesChildrenExpiration <= to && a.ElectrodesChildrenExpiration >= from)
            ).OfType<Aed>()
            .ToListAsync();

        var personalAeds = await _dbContext.PersonalAeds
            .Where(a => (a.BatteryExpiration <= to && a.BatteryExpiration >= from) ||
                        (a.ElectrodesAdultsExpiration <= to && a.ElectrodesAdultsExpiration >= from) ||
                        (a.ElectrodesChildrenExpiration <= to && a.ElectrodesChildrenExpiration >= from)
            ).Join(
                _dbContext.Users,
                a => a.OwnerId,
                u => u.Id,
                (a, u) => new { Aed = a, User = u }
            ).ToListAsync();

        var personalAedsWithOwners = personalAeds.Select(result =>
        {
            result.Aed.Owner = result.User.ToDomainUser();
            return (Aed)result.Aed;
        }).ToList();
        
        var aeds = publicAeds.Concat(personalAedsWithOwners).ToList();
                    
        var aedEvents = new List<AedEventDTO>();
        
        foreach (var aed in aeds)
        {
            if (aed.BatteryExpiration != null && aed.BatteryExpiration <= to && aed.BatteryExpiration >= from)
            {
                aedEvents.Add(new AedEventDTO
                {
                    AedId = aed.Id,
                    EventDate = aed.BatteryExpiration.Value,
                    HolderName = aed.GetDisplayHolder(),
                    Type = "BatteryExpiration"
                });
            }
            
            if (aed.ElectrodesAdultsExpiration != null && aed.ElectrodesAdultsExpiration <= to && aed.ElectrodesAdultsExpiration >= from)
            {
                aedEvents.Add(new AedEventDTO
                {
                    AedId = aed.Id,
                    EventDate = aed.ElectrodesAdultsExpiration.Value,
                    HolderName = aed.GetDisplayHolder(),
                    Type = "ElectrodesAdultsExpiration"
                });
            }
            
            if (aed.ElectrodesChildrenExpiration != null && aed.ElectrodesChildrenExpiration <= to && aed.ElectrodesChildrenExpiration >= from)
            {
                aedEvents.Add(new AedEventDTO
                {
                    AedId = aed.Id,
                    EventDate = aed.ElectrodesChildrenExpiration.Value,
                    HolderName = aed.GetDisplayHolder(),
                    Type = "ElectrodesChildrenExpiration"
                });
            }
        }
        
        return aedEvents;
    }
}