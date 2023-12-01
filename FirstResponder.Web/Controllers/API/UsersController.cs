using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Policy = "IsEmployee")]
    public async Task<IEnumerable<UserSearchResultDTO>> Search(string query)
    {
        var searchResults = await _mediator.Send(new GetUsersBySearchQuery(query));
        return searchResults;
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    [Authorize(Policy = "IsEmployee")]
    public async Task<IEnumerable<UserItemDTO>> FilteredTableItems(int pageNumber, [FromQuery] UserItemFiltersDTO filtersDto)
    {
        var items = await _mediator.Send(new GetUserItems() { PageNumber = pageNumber, Filters = filtersDto });
        return items;
    }
    
}