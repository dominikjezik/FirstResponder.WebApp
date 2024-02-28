using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
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
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Map()
    {
        return View();
    }

    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        await LoadOptionsForSelectionsToViewBag();
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(AedFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsForSelectionsToViewBag();
            return View(model);
        }
        
        try
        {
            await HandleFileUploads(model);
            
            var aed = await _mediator.Send(new CreateAedCommand(model.AedFormDTO));
            this.DisplaySuccessMessage("AED bolo úspešne vytvorené!");
            return RedirectToAction(nameof(Edit), "Aed", new { aedId = aed.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            await LoadOptionsForSelectionsToViewBag(model.AedFormDTO.ManufacturerId);
            return View(model);
        }
    }

    [Route("{aedId}")]
    public async Task<IActionResult> Edit(string aedId)
    {
        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));

        if (aed == null)
        {
            return NotFound();
        }
        
        await LoadOptionsForSelectionsToViewBag(aed.ManufacturerId);

        var model = new AedFormViewModel
        {
            AedFormDTO = aed.ToAedFormDTO()
        };
        
        return View(model);
    }

    [HttpPost]
    [Route("{aedId}")]
    public async Task<IActionResult> Edit(string aedId, AedFormViewModel model)
    {
        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));

        if (aed == null)
        {
            return NotFound();
        }
        
        model.AedFormDTO.AedId = aed.Id;
        
        if (!ModelState.IsValid)
        {
            model.AedFormDTO.CreatedAt = aed.CreatedAt;
            
            if (aed is PersonalAed personalAed)
            {
                model.AedFormDTO.Owner = personalAed.Owner;
            }
            
            await LoadOptionsForSelectionsToViewBag(model.AedFormDTO.ManufacturerId);
            return View(model);
        }

        try
        {
            await HandleFileUploads(model);
            
            var updatedAed = await _mediator.Send(new UpdateAedCommand(model.AedFormDTO, model.AedPhotosToDelete));
            this.DisplaySuccessMessage("AED bolo úspešne aktualizované!");
            return RedirectToAction(nameof(Edit), "Aed", new { aedId = updatedAed.Id });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            
            model.AedFormDTO.CreatedAt = aed.CreatedAt;
            if (aed is PersonalAed personalAed)
            {
                model.AedFormDTO.Owner = personalAed.Owner;
            }

            await LoadOptionsForSelectionsToViewBag(model.AedFormDTO.ManufacturerId);
            return View(model);
        }
    }

    [HttpPost]
    [Route("{aedId}/[action]")]
    public async Task<IActionResult> Delete(Guid aedId)
    {
        try
        {
            await _mediator.Send(new DeleteAedCommand(aedId));
            this.DisplaySuccessMessage("AED bolo úspešne odstránené!");
            return RedirectToAction(nameof(Index), "Aed");
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IEnumerable<Model>> Manufacturers()
    {
        var models = await _mediator.Send(new GetAllManufacturersQuery());
        return models.Select(m => new Model { Id = m.Id, Name = m.Name });
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IEnumerable<Model>> Models(string manufacturerId)
    {
        if (string.IsNullOrEmpty(manufacturerId))
        {
            return new List<Model>();
        }

        try
        {
            var models = await _mediator.Send(new GetAllModelsQuery { ManufacturerId = manufacturerId });
            return models.Select(m => new Model { Id = m.Id, Name = m.Name });
        } catch (EntityNotFoundException)
        {
            return new List<Model>();
        }
    }

    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<AedItemDTO>> FilteredTableItems(int pageNumber, [FromQuery] AedItemFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetAedItemsQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
    
    [HttpGet]
    [Route("map-items")]
    public async Task<IEnumerable<AedItemDTO>> MapItems()
    {
        return await _mediator.Send(new GetAllPublicAedItemsQuery());
    }

    #region Helpers

    private async Task HandleFileUploads(AedFormViewModel model)
    {
        if (model.AedPhotoFormFiles != null)
        {
            model.AedFormDTO.AedPhotoFileUploadDTOs = new List<FileUploadDto>();
                
            foreach (var file in model.AedPhotoFormFiles)
            {
                var uploadDto = new FileUploadDto 
                { 
                    Extension = Path.GetExtension(file.FileName),
                    FileStream = file.OpenReadStream()
                };
                    
                model.AedFormDTO.AedPhotoFileUploadDTOs.Add(uploadDto);
            }
        }
    }
    
    private async Task LoadOptionsForSelectionsToViewBag(Guid? manufacturerId = null)
    {
        await LoadManufacturersToViewBag();
        await LoadModelsToViewBag(manufacturerId);
        await LoadLanguagesToViewBag();
    }

    private async Task LoadManufacturersToViewBag()
    {
        ViewBag.Manufacturers = (await _mediator.Send(new GetAllManufacturersQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
    }
    
    private async Task LoadModelsToViewBag(Guid? manufacturerId = null)
    {
        if (manufacturerId == null)
        {
            ViewBag.Models = new List<SelectListItem>();
            return;
        }

        try
        {
            ViewBag.Models = (await _mediator.Send(new GetAllModelsQuery{ ManufacturerId = manufacturerId.ToString() }))
                .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
        }
        catch (EntityNotFoundException)
        {
            ViewBag.Models = new List<SelectListItem>();
        }
    }

    private async Task LoadLanguagesToViewBag()
    {
        ViewBag.Languages = (await _mediator.Send(new GetAllLanguagesQuery()))
            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
    }

    #endregion
    
}