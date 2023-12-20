using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using FirstResponder.Web.Extensions;
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
    
    public async Task AddUser(User user, string password)
    {
        var applicationUser = user.ToApplicationUser();
        applicationUser.UserName = applicationUser.Email;
        
        var result = await _userManager.CreateAsync(applicationUser, password);

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

    public Task DeleteUser(User user)
    {
        throw new NotImplementedException();
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
                UserId = a.Id,
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

    public async Task<IEnumerable<UserWithGroupInfoDTO>> GetUsersWithGroupInfoAsync(Guid groupId, string searchQuery = "", bool includeNotInGroup = false)
    {
        var result = await _dbContext.Users
            .GroupJoin(
                _dbContext.GroupUser.Where(groupUser => groupUser.GroupId == groupId),
                user => user.Id,
                groupUser => groupUser.UserId,
                (user, groupUsers) => new { User = user, GroupUsers = groupUsers }
            )
            .Where(userGroup => userGroup.User.FullName.Contains(searchQuery))
            .Where(userGroup => includeNotInGroup || userGroup.GroupUsers.Any())
            .SelectMany(
                userGroup => userGroup.GroupUsers.DefaultIfEmpty(),
                (userGroup, groupUser) => new UserWithGroupInfoDTO
                {
                    UserId = userGroup.User.Id,
                    FullName = userGroup.User.FullName,
                    IsInGroup = groupUser != null
                }
            )
            .ToListAsync();
        
        return result;
    }


    #region Helpers

    private void HandleIdentityErrors(IdentityResult result)
    {
        var errors = new Dictionary<string, string>();
        
        foreach (var error in result.Errors)
        {
            // Odstranenie duplicitnej chyby, ktoru vyhodi Identity ak sa pokusame vytvorit uzivatela s rovnakym emailom
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