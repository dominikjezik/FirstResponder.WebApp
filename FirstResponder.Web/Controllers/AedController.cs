using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Shared;
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
        await LoadManufacturersToViewBag();
        await LoadModelsToViewBag();
        
        var aeds = await _mediator.Send(new GetAllAedsQuery());
        return View(aeds);
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
            TempData["SuccessMessage"] = "AED bolo úspešne vytvorené!";
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
            TempData["SuccessMessage"] = "AED bolo úspešne aktualizované!";
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
    public async Task<IActionResult> Delete(string aedId)
    {
        try
        {
            await _mediator.Send(new DeleteAedCommand(aedId));
            TempData["SuccessMessage"] = "AED bolo úspešne odstránené!";
            return RedirectToAction(nameof(Index), "Aed");
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [Route("[action]")]
    public async Task<IActionResult> Map()
    {
        var publicAeds = await _mediator.Send(new GetAllPublicAedsQuery());
        return View(publicAeds);
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

    #region Helpers

    private async Task HandleFileUploads(AedFormViewModel model)
    {
        if (model.AedPhotoFormFiles != null)
        {
            model.AedFormDTO.AedPhotoFileUploadDTOs = new List<FileUploadDTO>();
                
            foreach (var file in model.AedPhotoFormFiles)
            {
                var uploadDto = new FileUploadDTO 
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
        
        ViewBag.Models = (await _mediator.Send(new GetAllModelsQuery{ ManufacturerId = manufacturerId.ToString() }))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
    }

    private async Task LoadLanguagesToViewBag()
    {
        ViewBag.Languages = (await _mediator.Send(new GetAllLanguagesQuery()))
            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
    }

    #endregion
    
}