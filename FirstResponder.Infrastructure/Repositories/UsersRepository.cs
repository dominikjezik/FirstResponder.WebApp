using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.Infrastructure.DbContext;
using FirstResponder.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var applicationUsers = await _dbContext.Users.ToListAsync();
        var users = applicationUsers.Select(user => user.ToDomainUser());
        return users;
    }

    public async Task AddUser(User user)
    {
        var applicationUser = user.ToApplicationUser();
        _dbContext.Users.Add(applicationUser);
        user.Id = applicationUser.Id;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var applicationUser = await _dbContext.Users.Where(a => a.Id == id).FirstOrDefaultAsync();
        return applicationUser.ToDomainUser();
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
        
        _dbContext.Update(applicationUser);
        await _dbContext.SaveChangesAsync();
    }

    public Task DeleteUser(User user)
    {
        throw new NotImplementedException();
    }
}