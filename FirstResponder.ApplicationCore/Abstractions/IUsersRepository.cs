using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Users.DTOs;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IUsersRepository
{
    Task<IEnumerable<User>> GetAllUsers();

    Task AddUser(User user);

    Task<User?> GetUserById(Guid? id);
    
    Task UpdateUser(User user);

    Task DeleteUser(User user);
    
    Task<IEnumerable<UserSearchResultDTO>> GetUsersBySearch(string searchQuery, int limitResultsCount);
    
    Task<bool> UserExists(Guid? id);
}