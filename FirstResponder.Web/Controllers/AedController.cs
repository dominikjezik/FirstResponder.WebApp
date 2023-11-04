using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstResponder.Web.Controllers;

[Route("[controller]")]
public class AedController : Controller
{
    private readonly IMediator _mediator;

    public AedController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Manufacturers = (await _mediator.Send(new GetAllManufacturersQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        
        ViewBag.Models = (await _mediator.Send(new GetAllModelsQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        
        var aeds = await _mediator.Send(new GetAllAedsQuery());
        return View(aeds);
    }

    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Manufacturers = (await _mediator.Send(new GetAllManufacturersQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        
        ViewBag.Models = (await _mediator.Send(new GetAllModelsQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        
        ViewBag.Languages = (await _mediator.Send(new GetAllLanguagesQuery()))
            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
        
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(CreateAedDTO model)
    {
        var result = await _mediator.Send(new CreateAedCommand(model));
        return Ok(result);
    }

    [Route("{aedId}")]
    public IActionResult Details(string aedId)
    {
        return View();
    }
    
    [Route("[action]")]
    public IActionResult Map()
    {
        return View();
    }
}