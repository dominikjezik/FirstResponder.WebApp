using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace FirstResponder.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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

    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, bool refreshSignIn = false)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
        {
            throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string>();
        
            foreach (var error in result.Errors)
            {
                // Ignore duplicate error that Identity throws if we try to create a user with the same email
                if (error.Code == "DuplicateUserName" && error.Description.Contains("is already taken."))
                {
                    continue;
                }
                errors.Add(error.Code, error.Description);
            }

            throw new EntityValidationException(errors);
        }
        
        if (refreshSignIn)
        {
            await _signInManager.RefreshSignInAsync(user);
        }
    }

    public async Task RefreshSignInAsync(Guid userId)
    {
        var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
        await _signInManager.RefreshSignInAsync(applicationUser);
    }
}