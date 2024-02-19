using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IUsersRepository
{
    Task<User?> GetUserById(Guid? id);
    
    Task<UserDTO?> GetUserWithDetailsById(Guid? id);
    
    Task<IEnumerable<UserItemDTO>> GetUserItems(int pageNumber, int pageSize, UserItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<UserSearchResultDTO>> GetUsersBySearch(string searchQuery, int limitResultsCount);
    
    Task<bool> UserExists(Guid? id);
    
    Task AddUser(User user, string password);
    
    Task UpdateUser(User user);

    Task DeleteUser(User user);
}