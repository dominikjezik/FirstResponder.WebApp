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
        var authToken = _tokenService.GenerateAccessToken(user);
        
        // Generate and store refresh token
        var refreshToken = await _tokenService.GenerateAndStoreRefreshToken(user);
        
        // Store device token
        if (!string.IsNullOrEmpty(model.DeviceToken))
        {
            await _mediator.Send(new StoreUserDeviceTokenCommand(user, model.DeviceToken));
        }
        
        return Ok(new
        {
            authToken, 
            refreshToken = refreshToken.Token, 
            refreshTokenExpires = refreshToken.Expires
        });
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(UserRegisterFormDTO model)
    {
        try
        {
            var user = await _mediator.Send(new CreateUserCommand(model));
            
            // Generate JWT token
            var authToken = _tokenService.GenerateAccessToken(user);
        
            // Generate and store refresh token
            var refreshToken = await _tokenService.GenerateAndStoreRefreshToken(user);
        
            // Store device token
            if (!string.IsNullOrEmpty(model.DeviceToken))
            {
                await _mediator.Send(new StoreUserDeviceTokenCommand(user, model.DeviceToken));
            }
        
            return Ok(new
            {
                user, 
                authToken, 
                refreshToken = refreshToken.Token, 
                refreshTokenExpires = refreshToken.Expires
            });
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenViewModel refreshToken)
    {
        // Get (expired) JWT token from request
        var jwtToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(jwtToken))
        {
            return Unauthorized();
        }
        
        // Get user id from JWT token without expiration validation
        var userId = _tokenService.GetUserIdFromExpiredToken(jwtToken);
        
        if (userId == null)
        {
            return Unauthorized();
        }
        
        // Get refresh token model
        var refreshTokenModel = await _tokenService.GetRefreshTokenModel(refreshToken.RefreshToken, userId.Value);
        
        if (refreshTokenModel == null || refreshTokenModel.UserId != userId)
        {
            return Unauthorized("Invalid refresh token");
        }
        
        // Check if refresh token is expired
        if (refreshTokenModel.Expires < DateTime.UtcNow)
        {
            // Delete refresh token
            await _tokenService.DeleteRefreshToken(refreshTokenModel);
            return Unauthorized("Refresh token expired");
        }
        
        var user = await _mediator.Send(new GetUserEntityByIdQuery(userId.Value));
        
        if (user == null)
        {
            return Unauthorized();
        }
        
        // Generate JWT token
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        
        // If refresh token is about to expire, generate new one
        if (refreshTokenModel.Expires < DateTime.UtcNow.AddDays(7))
        {
            // Delete old refresh token
            await _tokenService.DeleteRefreshToken(refreshTokenModel);
            
            // Generate and store refresh token
            refreshTokenModel = await _tokenService.GenerateAndStoreRefreshToken(user);
        }
        
        return Ok(new
        {
            authToken = newAccessToken, 
            refreshToken = refreshTokenModel.Token, 
            refreshTokenExpires = refreshTokenModel.Expires
        });
    }
    
    [HttpPost]
    [Authorize("Bearer")]
    [Route("update-device-token")]
    public async Task<IActionResult> UpdateDeviceToken([FromBody] UpdateDeviceTokenViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        await _mediator.Send(new UpdateUserDeviceTokenCommand(userId, model.NewDeviceToken, model.OldDeviceToken));
        
        return Ok();
    }
    
    [HttpPost]
    [Authorize("Bearer")]
    [Route("logout")]
    public async Task<IActionResult> Logout(LogoutViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        await _mediator.Send(new LogoutUserCommand(userId, model.RefreshToken, model.DeviceToken));
        
        return Ok();
    }
}