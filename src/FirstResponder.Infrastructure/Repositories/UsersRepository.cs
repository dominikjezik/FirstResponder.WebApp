using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Common.Extensions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    public async Task<User?> GetUserById(Guid? id)
    {
        if (id == null)
        {
            return null;
        }
        
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());

        return applicationUser.ToDomainUser();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .Where(user => user.Email == email)
            .Select(applicationUser => applicationUser.ToDomainUser())
            .FirstOrDefaultAsync();
    }

    public async Task<UserProfileDTO?> GetUserProfileById(Guid? id)
    {
        return await _dbContext.Users
            .Where(user => user.Id == id)
            .Select(applicationUser => new UserProfileDTO
            {
                Email = applicationUser.Email,
                FullName = applicationUser.FullName,
                PhoneNumber = applicationUser.PhoneNumber,
                DateOfBirth = applicationUser.DateOfBirth,
                Address = applicationUser.Address,
                PostalCode = applicationUser.PostalCode,
                City = applicationUser.City,
                Region = applicationUser.Region,
                Notes = applicationUser.Notes,
                
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UserDTO?> GetUserWithDetailsById(Guid? id)
    {
        if (id == null)
        {
            return null;
        }
        
        return await _dbContext.Users
            .Where(user => user.Id == id)
            .Include(user => user.Groups)
                .ThenInclude(groupUser => groupUser.Group)
            .Include(user => user.Incidents
                    .Where(incidentResponder => incidentResponder.AcceptedAt != null)
                    .OrderByDescending(incidentResponder => incidentResponder.CreatedAt))
                .ThenInclude(incidentResponder => incidentResponder.Incident)
            .Include(user => user.Courses.OrderByDescending(c => c.Course.CreatedAt))
                .ThenInclude(courseUser => courseUser.Course)
            .Select(applicationUser => applicationUser.ToUserDTO())
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<UserItemDTO>> GetUserItems(int pageNumber, int pageSize, UserItemFiltersDTO? filtersDTO = null)
    {
        var userItemsQueryable = _dbContext.Users
            .OrderByDescending(u => u.CreatedAt)
            .AsQueryable();

        if (filtersDTO != null)
        {
            userItemsQueryable = userItemsQueryable
                .Where(u =>
                    (string.IsNullOrEmpty(filtersDTO.FullName) ||u.FullName.Contains(filtersDTO.FullName)) &&
                    (string.IsNullOrEmpty(filtersDTO.PhoneNumber) || u.PhoneNumber.Contains(filtersDTO.PhoneNumber)) &&
                    (filtersDTO.Type == null || filtersDTO.Type == u.Type) &&
                    (filtersDTO.Region == null || filtersDTO.Region == u.Region)
                );
        }
        
        return await userItemsQueryable
            .Select(u => new UserItemDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt.ToString("dd.MM.yyyy HH:mm").ToUpper(),
                Type = u.Type.GetDisplayAttributeValue(),
                Region = u.Region.GetDisplayAttributeValue(),
                Address = u.Address,
            })
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<UserSearchResultDTO>> GetUsersBySearch(string searchQuery, int limitResultsCount)
    {
        var results = await _dbContext.Users
            .Where(a => 
                a.FullName.Contains(searchQuery) || 
                a.Email.Contains(searchQuery)
            )
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new UserSearchResultDTO
            {
                Id = a.Id,
                FullName = a.FullName,
                Email = a.Email
            })
            .Take(limitResultsCount)
            .ToListAsync();
        
        return results;
    }

    public async Task<bool> UserExists(Guid? id)
    {
        return await _dbContext.Users.Where(a => a.Id == id).CountAsync() == 1;
    }

    public async Task AddUser(User user, string? password = null)
    {
        var applicationUser = user.ToApplicationUser();
        applicationUser.UserName = applicationUser.Email;
        
        IdentityResult result;
        
        if (password == null)
        {
            result = await _userManager.CreateAsync(applicationUser);
        }
        else
        {
            result = await _userManager.CreateAsync(applicationUser, password);
        }

        if (!result.Succeeded)
        {
            HandleIdentityErrors(result);
        }
    
        user.Id = applicationUser.Id;
        user.CreatedAt = applicationUser.CreatedAt;
        user.UpdatedAt = applicationUser.UpdatedAt;
    }

    public async Task UpdateUser(User user)
    {
        var applicationUser = await _dbContext.Users.Where(a => a.Id == user.Id).FirstOrDefaultAsync();

        if (applicationUser == null)
        {
            throw new EntityNotFoundException();
        }
        
        applicationUser.FullName = user.FullName;
        applicationUser.Email = user.Email;
        applicationUser.UserName = user.Email;
        applicationUser.PhoneNumber = user.PhoneNumber;
        applicationUser.DateOfBirth = user.DateOfBirth;
        applicationUser.Address = user.Address;
        applicationUser.PostalCode = user.PostalCode;
        applicationUser.City = user.City;
        applicationUser.Region = user.Region;
        applicationUser.Notes = user.Notes;
        applicationUser.Type = user.Type;

        var result = await _userManager.UpdateAsync(applicationUser);
        
        if (!result.Succeeded)
        {
            HandleIdentityErrors(result);
        }
    }

    public async Task DeleteUser(User user)
    {
        var applicationUser = await _dbContext.Users.Where(a => a.Id == user.Id).FirstOrDefaultAsync();

        if (applicationUser == null)
        {
            throw new EntityNotFoundException();
        }

        var result = await _userManager.DeleteAsync(applicationUser);
        
        if (!result.Succeeded)
        {
            HandleIdentityErrors(result);
        }
    }

    #region Helpers

    private void HandleIdentityErrors(IdentityResult result)
    {
        var errors = new Dictionary<string, string>();
        
        foreach (var error in result.Errors)
        {
            // Ignore duplicate error that Identity throws if we try to create a user with the same email
            if (error.Code == "DuplicateUserName" && error.Description.Contains("is already taken."))
            {
                continue;
            }
            errors.Add(error.Code, error.Description);
        }

        throw new EntityValidationException(errors);
    }

    #endregion
    
}