using FirstResponder.ApplicationCore.Abstractions;
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

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}