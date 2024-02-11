namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IAuthService
{
    Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent);

    Task SignOutAsync();
    
    
}