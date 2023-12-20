using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Users.DTOs;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IUsersRepository
{
    Task AddUser(User user, string password);

    Task<User?> GetUserById(Guid? id);
    
    Task UpdateUser(User user);

    Task DeleteUser(User user);
    
    Task<IEnumerable<UserSearchResultDTO>> GetUsersBySearch(string searchQuery, int limitResultsCount);
    
    Task<bool> UserExists(Guid? id);
    
    Task<IEnumerable<UserItemDTO>> GetUserItems(int pageNumber, int pageSize, UserItemFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<UserWithGroupInfoDTO>> GetUsersWithGroupInfoAsync(Guid groupId, string searchQuery = "", bool includeNotInGroup = false);
}