using FirstResponder.ApplicationCore.Entities;

namespace FirstResponder.ApplicationCore.Abstractions;

public interface IUsersRepository
{
    Task<IEnumerable<User>> GetAllUsers();

    Task AddUser(User user);

    Task<User?> GetUserById(Guid id);
    
    Task UpdateUser(User user);

    Task DeleteUser(User user);
}