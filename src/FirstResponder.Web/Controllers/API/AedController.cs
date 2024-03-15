using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Route("api/[controller]")]
public class AedController : ApiController
{
    private readonly IMediator _mediator;

    public AedController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize("IsEmployee")]
    [Route("")]
    public async Task<IEnumerable<AedItemDTO>> GetAeds([FromQuery] int pageNumber, [FromQuery] AedItemFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetAedItemsQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
}