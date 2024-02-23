using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Register(UserRegisterFormDTO model)
    {
        try
        {
            var user = await _mediator.Send(new CreateUserCommand(model));
            
            // Generate JWT token
            var token = _tokenService.GenerateToken(user);
            return Ok(new { user, token });
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
    }
    
    [HttpGet]
    [Authorize("Bearer")]
    [Route("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var user = await _mediator.Send(new GetUserProfileByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
    
    [HttpPut]
    [Authorize("Bearer")]
    [Route("profile")]
    public async Task<IActionResult> UpdateProfile(UserProfileDTO model)
    {
        var validGuid = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        
        if (!validGuid)
        {
            return BadRequest("Invalid user id");
        }

        try
        {
            await _mediator.Send(new UpdateUserCommand(userId, model));
        }
        catch(EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
        
        return Ok();
    }
    
}