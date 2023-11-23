using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Enums;
using FirstResponder.ApplicationCore.Exceptions;
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
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var applicationUsers = await _dbContext.Users.ToListAsync();
        var users = applicationUsers.Select(user => user.ToDomainUser());
        return users;
    }

    public async Task<User?> GetUserById(Guid? id)
    {
        if (id == null)
        {
            return null;
        }
        
        var applicationUser = await _userManager.FindByIdAsync(id.ToString());
        if (applicationUser == null)
        {
            return null;
        }
        
        var user = applicationUser.ToDomainUser();
        
        var roles = await _userManager.GetRolesAsync(applicationUser);

        if (roles.Count != 0)
        {
            user.Type = Enum.Parse<UserType>(roles.First());
        }

        return user;
    }
    
    public async Task AddUser(User user, string password)
    {
        var applicationUser = user.ToApplicationUser();
        applicationUser.UserName = applicationUser.Email;
        
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var result = await _userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
            {
                var errors = new Dictionary<string, string>();
            
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Code, error.Description);
                }

                throw new EntityValidationException(errors);
            }
        
            user.Id = applicationUser.Id;
        
            if (user.Type != UserType.Default)
            {
                await _userManager.AddToRoleAsync(applicationUser, user.Type.ToString());
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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
        applicationUser.PhoneNumber = user.PhoneNumber;
        applicationUser.DateOfBirth = user.DateOfBirth;
        applicationUser.Address = user.Address;
        applicationUser.PostalCode = user.PostalCode;
        applicationUser.City = user.City;
        applicationUser.Region = user.Region;
        applicationUser.Notes = user.Notes;
      
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            _dbContext.Update(applicationUser);

            await this.UpdateUserRole(applicationUser, user.Type);
            await _dbContext.SaveChangesAsync();
            
            await transaction.CommitAsync();
        } 
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
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

    /**
     * This method is used to update user role in case that user type has changed.
     * It assumes that user has always only one role.
     */
    private async Task UpdateUserRole(ApplicationUser applicationUser, UserType newType)
    {
        var roles = await _userManager.GetRolesAsync(applicationUser);

        if (roles.Count != 0)
        {
            if (newType == UserType.Default)
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, roles.First());
            } 
            else if (roles.First() != newType.ToString())
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, roles.First());
                await _userManager.AddToRoleAsync(applicationUser, newType.ToString());
            }
        }
        else if (newType != UserType.Default)
        {
            await _userManager.AddToRoleAsync(applicationUser, newType.ToString());
        }
    }
    
}