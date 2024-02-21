using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IAuthService
{
    Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent);
    
    Task<User?> CheckPasswordSignInAsync(string userName, string password);

    Task SignOutAsync();

    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, bool refreshSignIn = false);

    Task RefreshSignInAsync(Guid userId);
}