using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace FirstResponder.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent)
    {
        var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: isPersistent, lockoutOnFailure: false);

        return result.Succeeded;
    }

    public async Task<User?> CheckPasswordSignInAsync(string userName, string password)
    {
        var user = await _signInManager.UserManager.FindByNameAsync(userName);
        
        if (user == null)
        {
            return null;
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            return null;
        }

        return user.ToDomainUser();
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}