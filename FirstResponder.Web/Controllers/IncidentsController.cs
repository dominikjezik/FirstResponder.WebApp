using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Extentions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class IncidentsController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(IncidentFormDTO model)
    {
        throw new NotImplementedException();
    }
    
    [Route("{incidentId}")]
    public async Task<IActionResult> Edit(string incidentId)
    {
        return View();
    }
    
    [HttpPost]
    [Route("{incidentId}/[action]")]
    public async Task<IActionResult> Delete(string incidentId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<IncidentItemDTO>> FilteredTableItems(int pageNumber, [FromQuery] IncidentItemFiltersDTO filtersDto)
    {
        // TODO
        var items = new List<IncidentItemDTO>();
        
        // Sample item
        items.Add(new IncidentItemDTO
        {
            Id = Guid.Empty,
            CreatedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm").ToUpper(),
            ResolvedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm").ToUpper(),
            Patient = "John Doe",
            Address = "Lorem ipsum, 123",
            Diagnosis = "Lorem ipsum",
            State = IncidentState.Completed.GetDisplayAttributeValue(),
        });
        
        return items;
    }
    
    
}