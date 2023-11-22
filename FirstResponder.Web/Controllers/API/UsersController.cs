using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("search")]
    public async Task<IEnumerable<UserSearchResultDTO>> Search(string query)
    {
        var searchResults = await _mediator.Send(new GetUsersBySearchQuery(query));

        return searchResults;
    }
    
}