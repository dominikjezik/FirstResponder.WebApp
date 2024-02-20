using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Route("api/[controller]")]
public class AccountController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public AccountController(IMediator mediator, IAuthService authService, ITokenService tokenService)
    {
        _mediator = mediator;
        _authService = authService;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Authenticate user
        var user = await _authService.CheckPasswordSignInAsync(model.Email, model.Password);
        
        if (user == null)
        {
            return Unauthorized();
        }
        
        // Generate JWT token
        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(UserAuthFormDTO model)
    {
        model.UserType = UserType.Default;
        
        try
        {
            var user = await _mediator.Send(new CreateUserCommand(model, model.Password));
            
            // Generate JWT token
            var token = _tokenService.GenerateToken(user);
            return Ok(new { user, token });
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
    }
}