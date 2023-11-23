using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    #region Register

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register(UserAuthFormDTO model)
    {
        // TODO: Zabezpečiť aby rola používateľa bola vždy Nezaradený
        
        try
        {
            await _mediator.Send(new CreateUserCommand(model, model.Password));
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
        
        // TODO: Vrátiť API token

        return Ok();
    }

    #endregion
}