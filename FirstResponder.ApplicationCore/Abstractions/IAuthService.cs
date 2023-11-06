namespace FirstResponder.ApplicationCore.Abstractions;

public interface IAuthService
{
    Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent);

    Task SignOutAsync();
    
    
}